namespace Template.Infrastructure.Data.Seed.Configuration
{
    public class UserSeedOptions
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
