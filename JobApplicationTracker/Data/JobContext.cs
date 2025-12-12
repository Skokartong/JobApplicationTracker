using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data
{
    public class JobContext : DbContext
    {
        public JobContext(DbContextOptions<JobContext> options) : base(options)
        {
        }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<JobType> Types {get;set;}
        public DbSet<Address> Addresses {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobListing>()
                .HasOne(j => j.Employer)
                .WithMany(e => e.JobListings)
                .HasForeignKey(j => j.EmployerId);
        }
    }
}
