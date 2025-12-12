using JobApplicationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace JobApplicationTracker.Service
{
    public class JobSearchService : IJobSearchService
    {
        private readonly HttpClient _client;

        public JobSearchService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<JobListing>> GetJobListingsAsync(string searchTerm, string location)
        {
            var url = $"https://jobsearch.api.jobtechdev.se/search?query={searchTerm}&location={location}&limit=50"; 

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch job listings from JobTech.");
            }

            var content = await response.Content.ReadAsStringAsync();

            var jobListings = JsonConvert.DeserializeObject<List<JobListing>>(content)
                .Select(job => new JobListing
                {
                    Id = job.Id,
                    Title = job.Title,
                    Employer = new Employer
                    {
                        Name = job.Employer.Name 
                    },
                    Location = job.Location,
                    Description = job.Description,
                    EmploymentType = job.EmploymentType,
                    JobLevel = job.JobLevel,
                    PublishedDate = job.PublishedDate.ToUniversalTime(),
                    Url = job.Url,
                    CompanyDescription = job.CompanyDescription,
                    Requirements = job.Requirements
                }).ToList();

            return jobListings;
        }
    }
}
