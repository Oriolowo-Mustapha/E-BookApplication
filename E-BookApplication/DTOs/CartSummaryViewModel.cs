namespace E_BookApplication.DTOs
{
    public class CartSummaryViewModel
    {
        public IEnumerable<CartItemDTO> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
