using System.Runtime.CompilerServices;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Service;

public interface IJobSearchService
{
    Task<IEnumerable<JobListing>> GetJobListingsAsync(string searchTerm, string location, bool experienceNotRequired = false);
    Task<JobListing?> GetJobListingByIdAsync(string id);
}