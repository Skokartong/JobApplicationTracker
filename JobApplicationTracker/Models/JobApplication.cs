using JobApplicationTracker.Models.Enums;

namespace JobApplicationTracker.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public JobCategory JobCategory { get; set; }
        public int TitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        public EmploymentType Type { get; set; }
        public JobLevel Level { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Company { get; set; }
    }
}
