using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {        
        }

        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
    }
}
