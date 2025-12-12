using JobApplicationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
                throw new Exception("Failed to fetch job listings.");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            var result = JsonConvert.DeserializeObject<JobSearchResponse>(content);

            if (result == null || result.JobListings == null || !result.JobListings.Any())
            {
                return Enumerable.Empty<JobListing>();
            }

            if (result == null || result.JobListings == null || !result.JobListings.Any())
            {
                return Enumerable.Empty<JobListing>(); 
            }

            var jobListings = result.JobListings
                .Select(job => new JobListing
                {
                    Id = job.Id,
                    Title = job.Title, 
                    Url = job.Url, 
                    Requirements = job.Requirements,

                    Employer = new Employer
                    {
                        Name = job.Employer?.Name ?? "Unknown Employer"  
                    },

                    Address = new Address
                    {
                        City = job.Address?.City ?? "Unknown City", 
                        StreetAddress = job.Address?.StreetAddress ?? "Unknown Street",  
                        Postcode = job.Address?.Postcode ?? "Unknown Postcode",  
                        Country = job.Address?.Country ?? "Unknown Country"  
                    },

                    Description = job.Description != null ? new Description
                    {
                        Text = job.Description.Text ?? "No description available"  
                    } : null,

                    JobType = new JobType
                    {
                        Label = job.JobType?.Label ?? "Unknown Job Type" 
                    },

                    Category = job.Category != null ? new Category
                    {
                        Label = job.Category.Label ?? "Unknown Category"  
                    } : null
                })
                .ToArray();  

            return jobListings;
        }
    }
}
