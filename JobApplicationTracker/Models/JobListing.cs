using System.ComponentModel.DataAnnotations.Schema;
using JobApplicationTracker.Models.Enums;
using Newtonsoft.Json;

namespace JobApplicationTracker.Models;
    public class JobListing
    {
        [JsonProperty("id")]  
        public string Id { get; set; }

        [JsonProperty("title")]  
        public string Title { get; set; }
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }
        [JsonProperty("employer")]  
        public Employer Employer { get; set; }  

        [JsonProperty("location")]  
        public string Location { get; set; }

        [JsonProperty("description")]  
        public string Description { get; set; }

        [JsonProperty("employmentType")]  
        public string EmploymentType { get; set; }

        [JsonProperty("jobLevel")]  
        public string JobLevel { get; set; }

        [JsonProperty("publishedDate")]  
        public DateTime PublishedDate { get; set; }

        [JsonProperty("url")]  
        public string Url { get; set; }

        [JsonProperty("companyDescription")]  
        public string CompanyDescription { get; set; }

        [JsonProperty("requirements")]  
        public string Requirements { get; set; }

        public ApplicationStatus? Status { get; set; }
    }

    public class Employer
    {
        public int? Id {get;set;}
        [JsonProperty("name")]  
        public string Name { get; set; }
        public ICollection<JobListing> JobListings { get; set; }
    }
