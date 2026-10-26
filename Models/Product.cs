using System.ComponentModel.DataAnnotations;

namespace GestionArticles.Models
{
    // Modèle représentant un produit dans la base de données
    public class Product
    {
        // Identifiant unique du produit (clé primaire, généré automatiquement)
        public int ProductId { get; set; }

        // Nom du produit : obligatoire, maximum 50 caractères
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
        public string Name { get; set; } = string.Empty;

        // Prix du produit en dinar tunisien
        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Display(Name = "Prix (DT)")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif.")]
        public decimal Price { get; set; }

        // Quantité disponible en stock
        [Required(ErrorMessage = "La quantité est obligatoire.")]
        [Display(Name = "Quantité en stock")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive.")]
        public int QteStock { get; set; }

        // Clé étrangère vers la catégorie
        public int CategoryId { get; set; }

        // Navigation vers la catégorie associée (null si non chargée)
        public Category? Category { get; set; }

        // Nom du fichier image (stocké dans wwwroot/images)
        [Display(Name = "Image")]
        public string? Image { get; set; }
    }
}
