using System.ComponentModel.DataAnnotations;

namespace BlockChain.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
