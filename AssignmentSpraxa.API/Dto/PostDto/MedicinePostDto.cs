using System.ComponentModel.DataAnnotations;

namespace AssignmentSpraxa.API.Dto.PostDto
{
    public class MedicinePostDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Company { get; set; }

        public decimal Price { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int Stock { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
