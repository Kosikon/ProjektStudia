namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        // This property is not mapped to the database
        public string Token { get; set; }
    }
}
