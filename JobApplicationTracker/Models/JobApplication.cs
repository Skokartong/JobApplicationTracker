using JobApplicationTracker.Models.Enums;

namespace JobApplicationTracker.Models;

public class JobApplication
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public JobCategory JobCategory { get; set; }
    public string Title { get; set; }
    public EmploymentType Type { get; set; }
    public JobLevel Level { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
    public string City {get;set;}
    public string Company { get; set; }
}
