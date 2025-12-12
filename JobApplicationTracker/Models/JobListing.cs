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

    [JsonProperty("headline")]
    public string Title { get; set; }

    [ForeignKey("Employer")]
    public int? EmployerId { get; set; }

    [JsonProperty("employer")]
    public Employer Employer { get; set; }

    [ForeignKey("Address")]
    public int? AddressId { get; set; }

    [JsonProperty("workplace_address")]
    public Address Address { get; set; }

    [ForeignKey("JobType")]
    public int? JobTypeId { get; set; }

    [JsonProperty("employment_type")]
    public JobType JobType { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("requirements")]
    public string Requirements { get; set; }

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }

    [JsonProperty("occupation_field")]
    public Category? Category { get; set; }

    [ForeignKey("Description")]
    public int? DescriptionId { get; set; }

    [JsonProperty("description")]
    public Description Description { get; set; }

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

public class JobType
{
    public int? Id { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }
}

public class Address
{
    public int? Id { get; set; }

    [JsonProperty("municipality")]
    public string City { get; set; }

    [JsonProperty("street_address")]
    public string StreetAddress { get; set; }

    [JsonProperty("postcode")]
    public string Postcode { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }
}

public class Description
{
    public int? Id { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
}
