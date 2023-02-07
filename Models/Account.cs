using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSpicy.Models
{
    public class Account
    {
        public int ID { get; set; }
        public string Name{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Tell { get; set; }
        public string Address { get; set; }
        public string? Image { get; set; }

        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }
}
