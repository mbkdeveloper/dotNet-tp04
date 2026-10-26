using GestionArticles.Models;
using GestionArticles.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionArticles.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    // Contrôleur pour gérer les catégories (CRUD complet)
    public class CategoryController : Controller
    {
        // Repository injecté via le constructeur (injection de dépendances)
        readonly ICategoryRepository CategRepository;

        public CategoryController(ICategoryRepository categRepository)
        {
            CategRepository = categRepository;
        }
        [AllowAnonymous]
        // GET: /Category/Index  → Affiche toutes les catégories
        public ActionResult Index()
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;
            return View(categories);
        }

        // GET: /Category/Details/5  → Affiche les détails d'une catégorie
        public ActionResult Details(int id)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;
            var category = CategRepository.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: /Category/Create  → Affiche le formulaire de création
        public ActionResult Create()
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;
            return View();
        }

        // POST: /Category/Create  → Traite le formulaire de création
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category c)
        {
            if (ModelState.IsValid)
            {
                CategRepository.Add(c);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = CategRepository.GetAll();
            return View(c);
        }

        // GET: /Category/Edit/5  → Affiche le formulaire d'édition
        public ActionResult Edit(int id)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;
            var category = CategRepository.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: /Category/Edit/5  → Traite le formulaire d'édition
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                var category = CategRepository.Update(c);

                if (category != null)
                    return RedirectToAction(nameof(Index));

                return NotFound();
            }

            ViewData["Categories"] = CategRepository.GetAll();
            return View(c);
        }

        // GET: /Category/Delete/5  → Affiche la page de confirmation de suppression
        public ActionResult Delete(int id)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;
            var category = CategRepository.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: /Category/Delete/5  → Effectue la suppression
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            CategRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}