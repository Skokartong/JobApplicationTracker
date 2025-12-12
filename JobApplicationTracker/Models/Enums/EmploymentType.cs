namespace JobApplicationTracker.Models.Enums
{
    public enum EmploymentType
    {
        FullTime,
        PartTime,
        Internship,
        Seasonal
    }

    public static class EmploymentTypeExtension
    {
        public static EmploymentType MapIt(string type)
        {
            return type switch
            {
                "Vanlig anställning" => EmploymentType.FullTime,
                "Halvtid" => EmploymentType.PartTime,
                "Praktik" => EmploymentType.Internship,
                "Säsongsarbete" => EmploymentType.Seasonal,
                _ => EmploymentType.FullTime
            };
        }
    }
}
