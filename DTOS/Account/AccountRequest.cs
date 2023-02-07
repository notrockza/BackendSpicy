using System.ComponentModel.DataAnnotations;

namespace BackendSpicy.DTOS.Account
{
    public class AccountRequest
    {

        [Required]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Tell { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public int? RoleID { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
    }
}
