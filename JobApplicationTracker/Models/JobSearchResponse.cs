using JobApplicationTracker.Models;

using System.Collections.Generic;
using Newtonsoft.Json;

namespace JobApplicationTracker.Models
{
    public class JobSearchResponse
    {
        [JsonProperty("total")]
        public Total Total { get; set; }

        [JsonProperty("positions")]
        public int Positions { get; set; }

        [JsonProperty("hits")]
        public List<JobListing> JobListings { get; set; }
    }

    public class Total
    {
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}

