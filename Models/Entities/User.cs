using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceAPI.Models.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation properties (Dapper 不會自動載入，需要手動查詢)
        [Write(false)]
        public List<Category> Categories { get; set; } = new();
        
        [Write(false)]
        public List<Transaction> Transactions { get; set; } = new();
    }
}