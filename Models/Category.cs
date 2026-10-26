using System.ComponentModel.DataAnnotations;

namespace GestionArticles.Models
{
    // Modèle représentant une catégorie de produits dans la base de données
    public class Category
    {
        // Identifiant unique de la catégorie (clé primaire, généré automatiquement)
        public int CategoryId { get; set; }

        // Nom de la catégorie : obligatoire
        [Required(ErrorMessage = "Le nom de la catégorie est obligatoire.")]
        [Display(Name = "Nom de la catégorie")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string CategoryName { get; set; } = string.Empty;

        // Liste des produits appartenant à cette catégorie (navigation)
        public ICollection<Product>? Products { get; set; }
    }
}
