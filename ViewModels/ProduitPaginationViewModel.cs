using GestionArticles.Models;

namespace GestionArticles.ViewModels
{
    // Produits à afficher, page actuelle et nombre total de pages
    public class ProduitPaginationViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int PageActuelle { get; set; }
        public int TotalPages { get; set; }
    }
}
