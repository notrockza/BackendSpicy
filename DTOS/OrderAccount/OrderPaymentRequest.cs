namespace BackendSpicy.DTOS.OrderAccount
{
    public class OrderPaymentRequest
    {
        public string? ID { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
    }
}
