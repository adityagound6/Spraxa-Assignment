namespace AssignmentSpraxa.API.Dto.PostDto
{
    public class RegisterRequestPostDto
    {
        public string FullName { get; set; }
        public string? ExternalId { get; set; }
        public bool Status { get; set; } = true;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Provider { get; set; }
        public string[]? Roles { get; set; }
    }
}
