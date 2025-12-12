using System.Reflection.Metadata;
using System.Security.Claims;
using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using JobApplicationTracker.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Controllers
{
    public class JobController : Controller
    {
        //private readonly IJobSearchService _jobService;
        private readonly JobContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(JobContext context, ILogger<JobController> logger)
        {
            _context = context;
            _logger = logger;
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

            job.UserId = userId;

            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(job);
            }

            _context.JobApplications.Add(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Update(int id)
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
        public async Task<IActionResult> Update(int id, JobApplication job)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingJob = await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);

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
            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var job = await _context.JobApplications.FindAsync(id);

            if (job == null || job.UserId != userId)
                return NotFound();

            _context.JobApplications.Remove(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error,
                    "An error occurred: {Path}", exceptionFeature.Path);
            }

            return View();
        }

        // [Authorize]
        // public IActionResult Search(string searchTerm, string location)
        // {
        //     var jobListings = new List<JobListing>();

        //     if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(location))
        //     {
        //         jobListings = _jobService.GetJobListingsAsync(searchTerm, location).Result.ToList();
        //     }

        //     ViewBag.SearchTerm = searchTerm;
        //     ViewBag.Location = location;

        //     return View(jobListings);
        // }

        // [Authorize]
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> SaveToApplications(int jobId)
        // {
        //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //     if (userId == null)
        //         return BadRequest("User not found.");

        //     var job = await _context.JobListings.FindAsync(jobId);

        //     if (job == null)
        //         return NotFound();

        //     var jobApplication = new JobApplication
        //     {
        //         UserId = userId,
        //         Status = ApplicationStatus.Applied,
        //         Title = job.Title,
        //         JobCategory = JobCategoryExtension.MapIt(category)
                
        //     };

        //     _context.JobApplications.Add(jobApplication);
        //     await _context.SaveChangesAsync();

        //     return RedirectToAction("Dashboard");
        // }


        [Authorize]
        private void LoadDropdowns()
        {
            ViewBag.CategoryList = Enum.GetValues(typeof(JobCategory))
                .Cast<JobCategory>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            ViewBag.StatusList = Enum.GetValues(typeof(ApplicationStatus))
                .Cast<ApplicationStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                })
                .ToList();

            ViewBag.TypeList = Enum.GetValues(typeof(EmploymentType))
                .Cast<EmploymentType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(JobLevel))
                .Cast<JobLevel>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                })
                .ToList();
        }
    }
}
