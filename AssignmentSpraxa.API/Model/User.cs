using System.ComponentModel.DataAnnotations;

namespace AssignmentSpraxa.API.Model
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
            Status = true;
        }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        [MaxLength(50)]
        public string Provider { get; set; } = "Local";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? ExternalId { get; set; }
    }
}
