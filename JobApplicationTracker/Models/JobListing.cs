using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
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

    [JsonProperty("workplace_address")]
    public Address Address { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("employment_type")]
    public EmploymentType Type { get; set; }

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
    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
    [JsonProperty("occupation_field")]
    public Category? Category { get; set; }

    public ApplicationStatus? Status { get; set; }
}

public class Employer
{
    public int? Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    public ICollection<JobListing>? JobListings { get; set; }
}

public class Category
{
    public int? Id { get; set; }
    [JsonProperty("label")]
    public string Label { get; set; }
}

public class EmploymentType
{
    public int? Id { get; set; }
    [JsonProperty("label")]
    public string Type { get; set; }
}

public class Address
{
    public int? Id {get;set;}
    [JsonProperty("city")]
    public string City {get;set;}
}

