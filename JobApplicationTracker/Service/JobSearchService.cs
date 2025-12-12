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
                throw new Exception("Failed to fetch job listings.");
            }

            var content = await response.Content.ReadAsStringAsync();

            var jobListings = JsonConvert.DeserializeObject<List<JobListing>>(content)
                .Select(job => new JobListing
                {
                    Id = job.Id,
                    Title = job.Title,
                    Url = job.Url,
                    Requirements = job.Requirements,

                    Employer = new Employer
                    {
                        Name = job.Employer?.Name
                    },

                    Address = new Address
                    {
                        City = job.Address?.City,
                        StreetAddress = job.Address?.StreetAddress,
                        Postcode = job.Address?.Postcode,
                        Country = job.Address?.Country
                    },

                    Description = job.Description != null ? new Description
                    {
                        Text = job.Description.Text
                    } : null,

                    JobType = new JobType
                    {
                        Label = job.JobType?.Label
                    },

                    Category = job.Category != null ? new Category
                    {
                        Label = job.Category.Label
                    } : null
                }).ToList();

            return jobListings;
        }
    }
}
