namespace Cookbook.API.Configuration
{
    public class DatabaseSettings
    {
        public const string SectionName = "ConnectionStrings";
        public string CookbookDb { get; set; } = string.Empty;
    }
}
