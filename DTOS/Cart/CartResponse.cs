namespace BackendSpicy.DTOS.Cart
{
    public class CartResponse
    {
        public string ID { get; set; }
        //public Models.Customer Customer { get; set; }
        public Models.Product Product { get; set; }
        public int AmountProduct { get; set; }
        public string ImageProduct { get; set; }
        // Models.Product product ส่งตัวจริงเข้ามาก่อน
        static public CartResponse FromCart(Models.Cart cart)
        {
            // return ตัวมันเอง
            return new CartResponse
            {
                ID = cart.ID,
                AmountProduct = cart.AmountProduct,
                Product = cart.Product,
                ImageProduct = !string.IsNullOrEmpty(cart.Product.Image) ? "https://localhost:7286/" + "images/" + cart.Product.Image : "",
                //Customer = cartCustomer.Customer
            };
        }
    }
}
