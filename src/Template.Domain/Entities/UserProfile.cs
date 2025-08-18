namespace Template.Domain.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string FullName { get; set; }
    }
}
