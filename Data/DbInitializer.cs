using GestionArticles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GestionArticles.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                // Obtenir les services requis
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Appliquer les migrations en attente
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la migration: {ex.Message}");
                }

                // Initialiser les catégories par défaut si aucune n'existe
                if (!await context.Categories.AnyAsync())
                {
                    context.Categories.AddRange(
                        new Category { CategoryName = "Informatique" },
                        new Category { CategoryName = "Bureautique" },
                        new Category { CategoryName = "Accessoires" }
                    );
                    await context.SaveChangesAsync();
                    Console.WriteLine("Catégories initiales créées avec succès.");
                }

                // Créer les rôles s'ils n'existent pas
                string[] roles = { "Admin", "Manager", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                        Console.WriteLine($"Rôle '{role}' créé avec succès.");
                    }
                }

                // ── Créer ou réinitialiser l'utilisateur Administrateur ──
                string adminEmail = "admin@admin.fr";
                string adminPassword = "Ad123456";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        Console.WriteLine($"Utilisateur admin '{adminEmail}' créé avec succès avec le rôle Admin.");
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors de la création de l'utilisateur admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    adminUser.EmailConfirmed = true;
                    adminUser.LockoutEnd = null;
                    adminUser.AccessFailedCount = 0;

                    var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                    var resetResult = await userManager.ResetPasswordAsync(adminUser, token, adminPassword);
                    if (!resetResult.Succeeded)
                    {
                        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();
                        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, adminPassword);
                        await userManager.UpdateAsync(adminUser);
                    }

                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                    Console.WriteLine($"L'utilisateur '{adminEmail}' mis à jour avec le rôle Admin.");
                }

                // ── Créer ou réinitialiser l'utilisateur Manager ──
                string managerEmail = "example@example.fr";
                string managerPassword = "Ex123456";

                var managerUser = await userManager.FindByEmailAsync(managerEmail);
                if (managerUser == null)
                {
                    managerUser = new IdentityUser
                    {
                        UserName = managerEmail,
                        Email = managerEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(managerUser, managerPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(managerUser, "Manager");
                        Console.WriteLine($"Utilisateur manager '{managerEmail}' créé avec succès avec le rôle Manager.");
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors de la création du manager: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    managerUser.EmailConfirmed = true;
                    managerUser.LockoutEnd = null;
                    managerUser.AccessFailedCount = 0;

                    var token = await userManager.GeneratePasswordResetTokenAsync(managerUser);
                    var resetResult = await userManager.ResetPasswordAsync(managerUser, token, managerPassword);
                    if (!resetResult.Succeeded)
                    {
                        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();
                        managerUser.PasswordHash = passwordHasher.HashPassword(managerUser, managerPassword);
                        await userManager.UpdateAsync(managerUser);
                    }

                    if (!await userManager.IsInRoleAsync(managerUser, "Manager"))
                        await userManager.AddToRoleAsync(managerUser, "Manager");

                    Console.WriteLine($"L'utilisateur '{managerEmail}' mis à jour avec le rôle Manager.");
                }
            }
        }
    }
}
