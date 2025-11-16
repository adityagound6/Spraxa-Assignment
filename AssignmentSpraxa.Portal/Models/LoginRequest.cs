namespace AssignmentSpraxa.Portal.Models
{
    public class LoginRequest
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public string Provider { get; set; } = "local";
        public string? ExternalId { get; set; }
    }
}
