namespace main_prj.Models
{
    public class CheckoutViewModel
    {
        public User User { get; set; } = null!;
        public Order? Order { get; set; }
        public Cart Cart { get; set; } = null!;
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        public int Subtotal { get; set; }
        public int ShippingFee { get; set; }
        public int Total ()
        {
            return Subtotal + ShippingFee;
        }
    }
}
