using GestionArticles.Data;
using GestionArticles.Models;
using GestionArticles.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// =====================================================================
// Configuration de l'application ASP.NET Core MVC
// =====================================================================

var builder = WebApplication.CreateBuilder(args);

// --- Enregistrement des services ---

// Ajouter le support des contrôleurs avec vues (pattern MVC) et enregistrer le Binder personnalisé pour les décimaux
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
});

// Configurer la connexion à la base de données SQL Server
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ProductDBConnection")
    )
);

// Enregistrer les repositories (injection de dépendances)
// Scoped = une instance par requête HTTP
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Configurer Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Paramètres des mots de passe
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Lockout.AllowedForNewUsers = false;
});

// Configurer les cookies d'authentification
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// --- Construction de l'application ---
var app = builder.Build();

// --- Configuration du pipeline HTTP ---

if (!app.Environment.IsDevelopment())
{
    // En production : afficher une page d'erreur générique
    app.UseExceptionHandler("/Home/Error");
    // HSTS = forcer HTTPS pendant 30 jours (sécurité)
    app.UseHsts();
}

// Rediriger HTTP vers HTTPS automatiquement
app.UseHttpsRedirection();

// Servir les fichiers statiques (CSS, JS, images dans wwwroot)
app.UseStaticFiles();

// Activer le routage
app.UseRouting();

// Activer l'authentification et l'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Configurer la route par défaut : /Controller/Action/Id
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialiser la base de données avec les données par défaut (rôles et utilisateur admin)
await DbInitializer.InitializeAsync(app);

// Démarrer l'application
app.Run();