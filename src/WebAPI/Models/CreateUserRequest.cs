namespace WebAPI.Models
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
