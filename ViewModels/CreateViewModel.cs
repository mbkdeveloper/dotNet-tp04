using System.ComponentModel.DataAnnotations;

namespace GestionArticles.ViewModels
{
    // ViewModel pour le formulaire de création d'un produit
    // Un ViewModel est utilisé à la place du modèle direct pour gérer l'upload de fichier (IFormFile)
    public class CreateViewModel
    {
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

        // Catégorie sélectionnée (nullable pour permettre la validation "champ requis")
        [Required(ErrorMessage = "Veuillez sélectionner une catégorie.")]
        [Display(Name = "Catégorie")]
        public int? CategoryId { get; set; }

        // Fichier image sélectionné (optionnel)
        [Display(Name = "Image du produit")]
        public IFormFile? ImagePath { get; set; }
    }
}