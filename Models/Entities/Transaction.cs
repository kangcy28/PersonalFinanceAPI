using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceAPI.Models.Entities
{
    [Table("Transactions")]
    public class Transaction : BaseEntity
    {
        [Required]
        public decimal Amount { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty; // "Income" or "Expense"

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        [Write(false)]
        public Category? Category { get; set; }
        
        [Write(false)]
        public User? User { get; set; }
    }
}