using Microsoft.AspNetCore.Identity;

namespace GestionArticles.Models
{
    // Commande passée par un utilisateur
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        // Lien avec l'utilisateur dans Identity
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        // Liste des articles de la commande
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
