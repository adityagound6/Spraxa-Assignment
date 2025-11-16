namespace AssignmentSpraxa.API.Dto.PostDto
{
    public class LoginRequestPostDto
    {
        public string UserNameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Provider { get; set; } = null!;
    }
}
