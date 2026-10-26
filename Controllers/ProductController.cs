using GestionArticles.Models;
using GestionArticles.Models.Repositories;
using GestionArticles.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionArticles.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {
        readonly IProductRepository ProductRepository;
        readonly ICategoryRepository CategRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductController(
            IProductRepository ProdRepository,
            ICategoryRepository categRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            ProductRepository = ProdRepository;
            CategRepository = categRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: ProductController
        [AllowAnonymous]
        public IActionResult Index(int? categoryId, int page = 1)
        {
            int pageSize = 4; // Nombre de produits par page

            if (page < 1)
                page = 1;

            var categories = CategRepository.GetAll();

            // Passer les catégories à la vue
            ViewData["Categories"] = categories;

            // Récupérer les produits en fonction de categoryId, s'il est spécifié
            IQueryable<Product> productsQuery = ProductRepository.GetAllProducts();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
            }

            // Pagination
            var totalProducts = productsQuery.Count();
            var products = productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId; // Passer categoryId à la vue

            return View(products);
        }

        // GET: ProductController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;

            var product = ProductRepository.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;

            ViewBag.CategoryId = new SelectList(
                CategRepository.GetAll(),
                "CategoryId",
                "CategoryName"
            );

            return View();
        }

        // POST: ProductController/Create
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            // Validation supplémentaire : CategoryId doit être > 0
            if (model.CategoryId == null || model.CategoryId <= 0)
                ModelState.AddModelError("CategoryId", "Veuillez sélectionner une catégorie valide.");

            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;

                // Vérifier si une image a été sélectionnée
                if (model.ImagePath != null && model.ImagePath.Length > 0)
                {
                    // Le fichier sera enregistré dans wwwroot/images
                    string webRoot = hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string uploadsFolder = Path.Combine(
                        webRoot,
                        "images"
                    );

                    // Créer le dossier s'il n'existe pas
                    Directory.CreateDirectory(uploadsFolder);

                    // Générer un nom unique et sécurisé pour l'image
                    string safeFileName = Path.GetFileName(model.ImagePath.FileName);
                    uniqueFileName = Guid.NewGuid().ToString()
                        + "_"
                        + safeFileName;

                    // Chemin complet du fichier
                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName
                    );

                    // Copier l'image dans wwwroot/images (using pour libérer le fichier)
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImagePath.CopyTo(fileStream);
                    }
                }

                // Créer un nouveau produit
                Product newProduct = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    QteStock = model.QteStock,
                    CategoryId = model.CategoryId!.Value,

                    // Enregistrer le nom de l'image dans la base de données
                    Image = uniqueFileName
                };

                try
                {
                    // Ajouter le produit dans la base de données
                    ProductRepository.Add(newProduct);

                    return RedirectToAction(
                        "Details",
                        new { id = newProduct.ProductId }
                    );
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $"Erreur lors de l'enregistrement : {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            // Recharger la liste des catégories en cas d'erreur
            ViewData["Categories"] = CategRepository.GetAll();
            ViewBag.CategoryId = new SelectList(
                CategRepository.GetAll(),
                "CategoryId",
                "CategoryName",
                model.CategoryId
            );

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager")]

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;

            ViewBag.CategoryId = new SelectList(CategRepository.GetAll(),
            "CategoryId"
            ,
            "CategoryName");
            Product? product = ProductRepository.GetById(id);
            if (product == null)
                return NotFound();

            EditViewModel productEditViewModel = new EditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                QteStock = product.QteStock,
                CategoryId = product.CategoryId,
                ExistingImagePath = product.Image
            };
            return View(productEditViewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            ViewBag.CategoryId = new SelectList(CategRepository.GetAll(), "CategoryId", "CategoryName");
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the product being edited from the database
                Product? product = ProductRepository.GetById(model.ProductId);
                if (product == null)
                    return NotFound();

                // Update the product object with the data in the model object
                product.Name = model.Name;
                product.Price = model.Price;
                product.QteStock = model.QteStock;
                product.CategoryId = model.CategoryId;
                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.ImagePath != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (!string.IsNullOrEmpty(model.ExistingImagePath))
                    {
                        string webRoot = hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string filePath = Path.Combine(webRoot, "images", model.ExistingImagePath);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the product object which will be
                    // eventually saved in the database
                    product.Image = ProcessUploadedFile(model);
                }
                // Call update method on the repository service passing it the
                // product object to update the data in the database table
                Product? updatedProduct = ProductRepository.Update(product);
                if (updatedProduct != null)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            ViewData["Categories"] = CategRepository.GetAll();
            return View(model);
        }
        [NonAction]
        private string? ProcessUploadedFile(EditViewModel model)
        {
            string? uniqueFileName = null;
            if (model.ImagePath != null && model.ImagePath.Length > 0)
            {
                string webRoot = hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadsFolder = Path.Combine(webRoot, "images");
                Directory.CreateDirectory(uploadsFolder);
                string safeFileName = Path.GetFileName(model.ImagePath.FileName);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + safeFileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;

            var product = ProductRepository.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            ProductRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        // search (consultation de la liste : accessible à tous)
        [AllowAnonymous]
        public ActionResult Search(string val)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;

            var result = ProductRepository.FindByName(val);
            return View("Index", result);
        }
    }
}