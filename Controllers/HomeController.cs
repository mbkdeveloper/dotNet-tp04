using GestionArticles.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestionArticles.Controllers
{
    // Contrôleur pour les pages générales de l'application (Accueil, Erreurs)
    public class HomeController : Controller
    {
        // GET: /Home/Index  → Page d'accueil
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy  → Page de politique de confidentialité
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Error  → Page d'erreur (ne doit pas être mise en cache)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
