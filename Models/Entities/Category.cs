using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceAPI.Models.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty; // "Income" or "Expense"

        [MaxLength(7)]
        public string Color { get; set; } = "#6B7280";

        public bool IsActive { get; set; } = true;

        [Required]
        public int UserId { get; set; }

        // Navigation properties
        [Write(false)]
        public User? User { get; set; }
        
        [Write(false)]
        public List<Transaction> Transactions { get; set; } = new();
    }
}