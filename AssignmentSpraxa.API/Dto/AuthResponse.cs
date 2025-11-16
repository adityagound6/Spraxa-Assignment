namespace AssignmentSpraxa.API.Dto
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string UserId { get; set; } = null!;
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
