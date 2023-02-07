using System.ComponentModel.DataAnnotations;

namespace BackendSpicy.DTOS.Products
{
    public class ProductRequest
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "no more than {1} chars")]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "between {1}-{2}")]
        public int Stock { get; set; }

        [Required]
        [Range(0, 1_000_000, ErrorMessage = "between {1}-{2}")]
        public double Price { get; set; }

       
        [Required]
        [MaxLength(100, ErrorMessage = "no more than {1} chars")]
        public string Detail { get; set; }

        [Required]
        public int CategoryProductID { get; set; }

        public IFormFileCollection? FormFiles { get; set; }
    }
}
