using System.ComponentModel.DataAnnotations;

namespace AssignmentSpraxa.Portal.DTO
{
    public class MedicineCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Range(1, int.MaxValue)]
        public int Stock { get; set; }

        [Range(1, double.MaxValue)]
        public double Price { get; set; }
    }
}
