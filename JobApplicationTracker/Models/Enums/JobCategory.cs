namespace JobApplicationTracker.Models.Enums
{
    public enum JobCategory
    {
        Administration,
        Finance,
        IT,
        Technology,
        Engineering,
        Sales,
        Services,
        Healthcare,
        Media,
        Trades,
        Transport,
        Other
    }

    public static class JobCategoryExtension
    {
        public static JobCategory MapIt(string category)
        {
            return category switch
            {
                "Administration" => JobCategory.Administration,
                "Finance" => JobCategory.Finance,
                "IT" => JobCategory.IT,
                "Technology" => JobCategory.Technology,
                "Engineering" => JobCategory.Engineering,
                "Sales" => JobCategory.Sales,
                "Services" => JobCategory.Services,
                "Healthcare" => JobCategory.Healthcare,
                "Media" => JobCategory.Media,
                "Trades" => JobCategory.Trades,
                "Transport" => JobCategory.Transport,
                _ => JobCategory.Other 
            };
        }
    }
}