namespace Template.Application.Common.Settings
{
    public class DefaultUsersAndRolesOptions
    {
        public List<string> Roles { get; set; } = new();
        public List<UserSeedOptions> Users { get; set; } = new();
    }
}
