using System.ComponentModel.DataAnnotations;

namespace BlockChain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? NamaToko { get; set; }

        [Required]
        public required  string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public string? Kategori { get; set; }

        public string? LogoPath { get; set; }

        [Required]
        public required string KataSandi { get; set; }
    }
}
