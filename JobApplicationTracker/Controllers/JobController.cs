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
        private readonly JobContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(JobContext context, ILogger<JobController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Home()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("sub")?.Value;

            var Jobs = await _context.JobApplications
                .Include(j => j.JobCategory)
                .Include(j => j.JobTitle)
                .Where(j => j.UserId == userId)
                .ToListAsync();

            return View(Jobs);
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
        public async Task<IActionResult> Add(JobApplication Job)
        {
            var userId = User.FindFirst("sub")?.Value;
            Job.UserId = userId;

            var titleInput = Request.Form["JobTitle.Title"].ToString().Trim();
            var categoryInput = Request.Form["JobCategory.Category"].ToString().Trim();

            var existingTitle = await _context.JobTitles
                .FirstOrDefaultAsync(t => t.Title.ToLower() == titleInput.ToLower());

            if (existingTitle == null)
            {
                var newTitle = new JobTitle { Title = titleInput };
                _context.JobTitles.Add(newTitle);
                await _context.SaveChangesAsync();
                Job.TitleId = newTitle.Id;
            }
            else
            {
                Job.TitleId = existingTitle.Id;
            }

            var existingCategory = await _context.JobCategories
                .FirstOrDefaultAsync(c => c.Category.ToLower() == categoryInput.ToLower());

            if (existingCategory == null)
            {
                var newCategory = new JobCategory { Category = categoryInput };
                _context.JobCategories.Add(newCategory);
                await _context.SaveChangesAsync();
                Job.CategoryId = newCategory.Id;
            }
            else
            {
                Job.CategoryId = existingCategory.Id;
            }

            if (ModelState.IsValid)
            {
                _context.JobApplications.Add(Job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(Job);
        }

        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var userId = User.FindFirst("sub")?.Value;
            var Job = await _context.JobApplications.FindAsync(id);

            if (Job == null || Job.UserId != userId)
                return NotFound();

            LoadDropdowns();
            return View(Job);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, JobApplication Job)
        {
            var userId = User.FindFirst("sub")?.Value;
            var existingJob = await _context.JobApplications.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);

            if (existingJob == null || existingJob.UserId != userId)
                return BadRequest();

            Job.UserId = existingJob.UserId;

            var titleInput = Request.Form["JobTitle.Title"].ToString().Trim();
            var categoryInput = Request.Form["JobCategory.Category"].ToString().Trim();

            var existingTitle = await _context.JobTitles
                .FirstOrDefaultAsync(t => t.Title.ToLower() == titleInput.ToLower());

            if (existingTitle == null)
            {
                var newTitle = new JobTitle { Title = titleInput };
                _context.JobTitles.Add(newTitle);
                await _context.SaveChangesAsync();
                Job.TitleId = newTitle.Id;
            }
            else
            {
                Job.TitleId = existingTitle.Id;
            }

            var existingCategory = await _context.JobCategories
                .FirstOrDefaultAsync(c => c.Category.ToLower() == categoryInput.ToLower());

            if (existingCategory == null)
            {
                var newCategory = new JobCategory { Category = categoryInput };
                _context.JobCategories.Add(newCategory);
                await _context.SaveChangesAsync();
                Job.CategoryId = newCategory.Id;
            }
            else
            {
                Job.CategoryId = existingCategory.Id;
            }

            if (ModelState.IsValid)
            {
                _context.Update(Job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(Job);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst("sub")?.Value;
            var Job = await _context.JobApplications.FindAsync(id);

            if (Job == null || Job.UserId != userId)
                return NotFound();

            return View(Job);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirst("sub")?.Value;
            var Job = await _context.JobApplications.FindAsync(id);

            if (Job == null || Job.UserId != userId)
                return NotFound();

            _context.JobApplications.Remove(Job);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                var path = exceptionFeature.Path;
                var error = exceptionFeature.Error;

                _logger.LogError(error, "An error occurred: {Path}", path);
            }

            return View();
        }

        [Authorize]
        private void LoadDropdowns()
        {
            ViewBag.Categories = _context.JobCategories.ToList();
            ViewBag.Titles = _context.JobTitles.ToList();

            ViewBag.StatusList = Enum.GetValues(typeof(ApplicationStatus))
                .Cast<ApplicationStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();

            ViewBag.TypeList = Enum.GetValues(typeof(EmploymentType))
                .Cast<EmploymentType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(JobLevel))
                .Cast<JobLevel>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();
        }
    }
}
