using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// Account Model
    /// </summary>
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public string CostumerName { get; set; }
        public string CostumerEmail { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public decimal? CurrentBalance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
