using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using JobApplicationTracker.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ApplicationContext context, ILogger<ApplicationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _context.JobApplications
                .Include(j => j.JobCategory)
                .Include(j => j.JobTitle)
                .ToListAsync();

            return View(applications);
        }

        public IActionResult Add()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(JobApplication application)
        {
            var titleInput = Request.Form["JobTitle.Title"].ToString().Trim();
            var categoryInput = Request.Form["JobCategory.Category"].ToString().Trim();

            var existingTitle = await _context.JobTitles
                .FirstOrDefaultAsync(t => t.Title.ToLower() == titleInput.ToLower());

            if (existingTitle == null)
            {
                var newTitle = new JobTitle { Title = titleInput };
                _context.JobTitles.Add(newTitle);
                await _context.SaveChangesAsync();
                application.TitleId = newTitle.Id;
            }
            else
            {
                application.TitleId = existingTitle.Id;
            }

            var existingCategory = await _context.JobCategories
                .FirstOrDefaultAsync(c => c.Category.ToLower() == categoryInput.ToLower());

            if (existingCategory == null)
            {
                var newCategory = new JobCategory { Category = categoryInput };
                _context.JobCategories.Add(newCategory);
                await _context.SaveChangesAsync();
                application.CategoryId = newCategory.Id;
            }
            else
            {
                application.CategoryId = existingCategory.Id;
            }

            if (ModelState.IsValid)
            {
                application.AppliedDate = DateTime.Now;
                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(application);
        }

        public async Task<IActionResult> Update(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application == null)
                return NotFound();

            LoadDropdowns();
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, JobApplication application)
        {
            if (id != application.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(application);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application == null)
                return NotFound();

            return View(application);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application != null)
            {
                _context.JobApplications.Remove(application);
                await _context.SaveChangesAsync();
            }

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
