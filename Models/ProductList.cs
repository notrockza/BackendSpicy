using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSpicy.Models
{
    public class ProductList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string OrderAccountID { get; set; }
        public int ProductID { get; set; }
        public int ProductPrice { get; set; }
        public int ProductAmount { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        [ForeignKey("OrderAccountID")]
        public virtual OrderAccount OrderAccount { get; set; }
    }
}
