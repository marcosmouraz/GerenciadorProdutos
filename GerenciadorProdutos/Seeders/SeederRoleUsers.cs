using Microsoft.AspNetCore.Identity;

namespace GerenciadorProdutos.Seeders;

public class SeederRoleUsers
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var roles = new[] { "Funcionário", "Gerente", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var users = new[]
        {
            new { Email = "admin@admin.com", Username = "admin", Password = "Admin123!", Role = "Admin" },
            new { Email = "gerente@empresa.com", Username = "gerente", Password = "Gerente123!", Role = "Gerente" },
            new { Email = "funcionario@empresa.com", Username = "funcionario", Password = "Funcionario123!", Role = "Funcionário" }
        };

        foreach (var userData in users)
        {
            var user = await userManager.FindByEmailAsync(userData.Email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userData.Username,
                    Email = userData.Email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user, userData.Password);

                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userData.Role);
                    Console.WriteLine($"Usuário {userData.Username} com role {userData.Role} criado com sucesso!");
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        Console.WriteLine($"Erro ao criar usuário {userData.Username}: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Usuário {userData.Username} já existe.");
            }
        }
    }
}
