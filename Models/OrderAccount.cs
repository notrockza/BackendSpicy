using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSpicy.Models
{
    public class OrderAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public bool PaymentStatus { get; set; }
        public string? ProofOfPayment { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int PriceTotal { get; set; }
        public bool AccountStatus { get; set; }
        public int AccountID { get; set; }
        [ForeignKey("AccountID")]
        //[ValidateNever]
        public virtual Account Account { get; set; }
    }
}
