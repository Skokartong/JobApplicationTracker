namespace JobApplicationTracker.Models
{
    public class JobCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public ICollection<JobApplication>? JobApplications { get; set; }
    }
}
