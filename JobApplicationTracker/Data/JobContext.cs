using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data
{
    public class JobContext:DbContext
    {
        public JobContext(DbContextOptions<JobContext> options):base(options)
        {        
        }

        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
    }
}
