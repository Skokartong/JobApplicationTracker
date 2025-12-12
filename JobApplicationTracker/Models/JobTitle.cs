namespace JobApplicationTracker.Models;
    public class JobTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<JobApplication>? JobApplications { get; set; }
    }
