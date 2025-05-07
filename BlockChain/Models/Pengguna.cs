using System.ComponentModel.DataAnnotations;

namespace BlockChain.Models
{
    public class Pengguna : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nama { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [StringLength(100)]
        public required string NamaToko { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Role { get; set; }

        // Validasi kustom untuk NamaToko jika Role == "Distributor"
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Role == "Distributor" && string.IsNullOrWhiteSpace(NamaToko))
            {
                yield return new ValidationResult(
                    "Nama Toko wajib diisi untuk Distributor.",
                    new[] { nameof(NamaToko) });
            }
        }
    }
}
