using System.ComponentModel.DataAnnotations;

namespace BackendSpicy.Models
{
    public class CategoryProduct
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
