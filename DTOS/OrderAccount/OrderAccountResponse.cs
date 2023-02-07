namespace BackendSpicy.DTOS.OrderAccount
{
    public class OrderAccountResponse
    {
        public string ID { get; set; }
        public bool PaymentStatus { get; set; }
        public string? ProofOfPayment { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int PriceTotal { get; set; }
        public bool AccountStatus { get; set; }
        public int AccountID { get; set; }
        static public OrderAccountResponse FromOrder(Models.OrderAccount orderAccount)
        {
            // return ตัวมันเอง
            return new OrderAccountResponse
            {
                ID = orderAccount.ID,
                PaymentStatus = orderAccount.PaymentStatus,
                ProofOfPayment = orderAccount.ProofOfPayment,
                //ProofOfPayment = !string.IsNullOrEmpty(orderAccount.ProofOfPayment) ? "https://localhost:7286/" + "images/" + orderAccount.ProofOfPayment : "",
                PriceTotal = orderAccount.PriceTotal,
                AccountStatus = orderAccount.AccountStatus,
                AccountID = orderAccount.AccountID,
            };
        }
    }
}
