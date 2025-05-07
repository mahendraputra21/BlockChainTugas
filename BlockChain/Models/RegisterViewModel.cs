using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BlockChain.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nama Toko wajib diisi")]
        public string? NamaToko { get; set; }

        [Required(ErrorMessage = "Username wajib diisi")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Hanya email gmail.com yang diperbolehkan")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Kategori wajib dipilih")]
        public string? Kategori { get; set; }

        [Display(Name = "Upload Logo")]
        public IFormFile? LogoFile { get; set; } // Untuk input upload file

        public string? LogoPath { get; set; } // Disimpan di database

        [Required(ErrorMessage = "Kata sandi wajib diisi")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Kata sandi minimal 6 karakter")]
        public string? KataSandi { get; set; }
    }
}
