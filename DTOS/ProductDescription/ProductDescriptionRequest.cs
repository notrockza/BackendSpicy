namespace BackendSpicy.DTOS.ProductDescription
{
    public class ProductDescriptionRequest
    {
        public string? ID { get; set; }
        //public string? Image { get; set; }
        public int ProductID { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
    }
}
