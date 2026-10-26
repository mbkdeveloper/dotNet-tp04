namespace GestionArticles.ViewModels
{
    public class OrderViewModel
    {
        public string Address { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    }
}
