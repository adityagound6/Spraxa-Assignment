namespace AssignmentSpraxa.Portal.DTO
{
    public class MedicineDto
    {
        public Guid MedicineId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
