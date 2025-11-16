namespace AssignmentSpraxa.Portal.Models
{
    public class RegisterRequest
    {
        public string? ExternalId { get; set; }
        public string? FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Provider { get; set; } = "local";
        public List<string> Roles { get; set; }
    }
}
