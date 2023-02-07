using System.ComponentModel.DataAnnotations;

namespace BackendSpicy.DTOS.Account
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }

        [Phone]
        public string Tell { get; set; }
        [Required]
        public string Address { get; set; }

        public IFormFileCollection? FormFiles { get; set; }
        public int RoleID { get; set; }
    }
}
