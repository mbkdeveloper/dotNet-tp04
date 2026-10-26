using System.ComponentModel.DataAnnotations;

namespace GestionArticles.ViewModels
{
    // ViewModel pour le formulaire de modification d'un produit
    // Contient aussi l'image existante pour pouvoir l'afficher et la remplacer
    public class EditViewModel
    {
        // Identifiant du produit à modifier (caché dans le formulaire)
        public int ProductId { get; set; }

        // Nom du produit : obligatoire, maximum 50 caractères
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
        [Display(Name = "Nom du produit")]
        public string Name { get; set; } = string.Empty;

        // Prix du produit
        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif.")]
        [Display(Name = "Prix (DT)")]
        public decimal Price { get; set; }

        // Quantité en stock
        [Required(ErrorMessage = "La quantité est obligatoire.")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être un nombre positif.")]
        [Display(Name = "Quantité en stock")]
        public int QteStock { get; set; }

        // Catégorie sélectionnée
        [Required(ErrorMessage = "Veuillez sélectionner une catégorie.")]
        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        // Nom du fichier image existant (pour l'affichage et la suppression en cas de changement)
        public string? ExistingImagePath { get; set; }

        // Nouveau fichier image sélectionné (optionnel)
        [Display(Name = "Changer l'image")]
        public IFormFile? ImagePath { get; set; }
    }
}