using System.ComponentModel.DataAnnotations;

namespace BackendSpicy.DTOS.Cart
{
    public class CartRequest
    {
        public string? ID { get; set; }
        [Required]
        public int AccountID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int AmountProduct { get; set; }
    }
}
