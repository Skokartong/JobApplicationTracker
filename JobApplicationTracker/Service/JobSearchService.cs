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
            var url = "https://jobsearch.api.jobtechdev.se/search?";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                url += $"q={Uri.EscapeDataString(searchTerm)}&";
            }

            if (!string.IsNullOrEmpty(location))
            {
                url += $"location={Uri.EscapeDataString(location)}&";
            }

            url += "limit=50";

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JobSearchResponse>(content);

            if (result == null || result.JobListings == null || !result.JobListings.Any())
            {
                return Enumerable.Empty<JobListing>();
            }

            return result.JobListings.Select(hit => new JobListing
            {
                Id = hit.Id,
                Title = hit.Title,
                Url = hit.Url,
                Employer = new Employer
                {
                    Name = hit.Employer.Name
                },
                Description = new Description
                {
                    Text = hit.Description?.Text
                },
                Address = new Address
                {
                    City = hit.Address.City,
                }
            });
        }
    }
}
