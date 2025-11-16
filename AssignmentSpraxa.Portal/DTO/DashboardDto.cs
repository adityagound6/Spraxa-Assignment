namespace AssignmentSpraxa.Portal.DTO
{
    public class DashboardDto
    {
        public User User { get; set; }
        public List<LoginHistory> LoginHistory { get; set; }
    }

    public class LoginHistory
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}
