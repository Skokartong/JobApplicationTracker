using System.Security.Claims;
using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using JobApplicationTracker.Models.Enums;
using JobApplicationTracker.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobSearchService _jobService;
        private readonly JobContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(JobContext context, ILogger<JobController> logger, IJobSearchService jobService)
        {
            _context = context;
            _logger = logger;
            _jobService = jobService;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var name = User.FindFirst("name")?.Value;

            var jobs = await _context.JobApplications
                .Where(j => j.UserId == userId)
                .ToListAsync();

            ViewBag.Name = name;
            return View(jobs);
        }

        [Authorize]
        public IActionResult Add()
        {
            LoadDropdowns();
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(JobApplication job)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId couldn't be found");

            job.Id = Guid.NewGuid().ToString();
            job.UserId = userId;

            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(job);
            }

            _context.JobApplications.Add(job);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Application added successfully! ✅";

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var job = await _context.JobApplications.FindAsync(id);

            if (job == null || job.UserId != userId)
                return NotFound();

            LoadDropdowns();
            return View(job);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(JobApplication job)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ModelState.Remove("UserId");

            var existingJob = await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == job.Id && j.UserId == userId);

            if (existingJob == null)
                return BadRequest();

            job.UserId = existingJob.UserId;

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(job);
            }

            _context.Entry(existingJob).CurrentValues.SetValues(job);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Job updated successfully! ✅";

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var job = await _context.JobApplications.FindAsync(id);

            if (job == null || job.UserId != userId)
                return NotFound();

            return View(job);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var job = await _context.JobApplications.FindAsync(id);

            if (job == null || job.UserId != userId)
                return NotFound();

            _context.JobApplications.Remove(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Search(string searchTerm, string location, bool experienceNotRequired = false, string sorting = "relevance")
        {
            IEnumerable<JobListing> jobListings = Enumerable.Empty<JobListing>();

            if (!string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(location) || experienceNotRequired || sorting != "relevance")
            {                
                jobListings = await _jobService.GetJobListingsAsync(searchTerm, location, experienceNotRequired, sorting)
                            ?? Enumerable.Empty<JobListing>();
            }

            var sortingOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "relevance", Text = "Relevance" },
                new SelectListItem { Value = "pubdate-desc", Text = "Newest First" },
                new SelectListItem { Value = "pubdate-asc", Text = "Oldest First" },
                new SelectListItem { Value = "applydate-desc", Text = "Newest Apply Date" },
                new SelectListItem { Value = "applydate-asc", Text = "Oldest Apply Date" },
                new SelectListItem { Value = "updated", Text = "Recently Updated" }
            };

            var selectedOption = sortingOptions.FirstOrDefault(o => o.Value == sorting);
            selectedOption?.Selected = true;

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Location = location;
            ViewBag.ExperienceNotRequired = experienceNotRequired;
            ViewBag.SortingOptions = sortingOptions;

            return View(jobListings);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveToApplications(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User not found.");

            var job = await _jobService.GetJobListingByIdAsync(id);

            if (job == null)
                return NotFound();

            var jobApplication = new JobApplication
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Status = ApplicationStatus.Applied,
                Title = job.Title,
                JobCategory = JobCategoryExtension.MapIt(job.Category.Label),
                Type = EmploymentTypeExtension.MapIt(job.JobType.Label),
                Level = JobLevel.Mid,
                City = job.Address?.City ?? "",
                Company = job.Employer?.Name ?? ""
            };

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        private void LoadDropdowns()
        {
            ViewBag.CategoryList = Enum.GetValues(typeof(JobCategory))
                .Cast<JobCategory>()
                .Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() })
                .ToList();

            ViewBag.StatusList = Enum.GetValues(typeof(ApplicationStatus))
                .Cast<ApplicationStatus>()
                .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
                .ToList();

            ViewBag.TypeList = Enum.GetValues(typeof(EmploymentType))
                .Cast<EmploymentType>()
                .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(JobLevel))
                .Cast<JobLevel>()
                .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
                .ToList();
        }
    }
}
