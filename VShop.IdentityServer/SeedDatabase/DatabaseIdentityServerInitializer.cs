using System.Security.Claims;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using VShop.IdentityServer.Configuration;
using VShop.IdentityServer.Data;

namespace VShop.IdentityServer.SeedDatabase;

public class DatabaseIdentityServerInitializer : IDatabaseSeedInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseIdentityServerInitializer(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void InitializeSeedRoles()
    {
        // se o perfil admin não existir então cria o perfil
        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
        {
            // cria o perfil admin
            IdentityRole adminRole = new IdentityRole();
            adminRole.Name = IdentityConfiguration.Admin;
            adminRole.NormalizedName = IdentityConfiguration.Admin.ToUpper();
            _roleManager.CreateAsync(adminRole).Wait();
        }

        // se o perfil cliente não existir então cria o perfil
        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Client).Result)
        {
            // cria o perfil client
            IdentityRole clientRole = new IdentityRole();
            clientRole.Name = IdentityConfiguration.Client;
            clientRole.NormalizedName = IdentityConfiguration.Client.ToUpper();
            _roleManager.CreateAsync(clientRole).Wait();
        }
    }

    public void InitializeSeedUsers()
    {
        // se o usuário admin não existir, cria o usuário, define a senha e atribiu ao perfil

        // define os dados do admin
        ApplicationUser admin = new ApplicationUser()
        {
            UserName = "admin1",
            NormalizedUserName = "ADMIN1",
            Email = "admin1@com.br",
            NormalizedEmail = "ADMIN@COM.BR",
            EmailConfirmed = true,
            LockoutEnabled = false,
            PhoneNumber = "+55 (11) 12345-6789",
            FirstName = "Usuario",
            LastName = "Admin1",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // cria o usuário admin e atribui a senha
        IdentityResult resultAdmin = _userManager.CreateAsync(admin, "Admin#2026").Result;
        if (resultAdmin.Succeeded)
        {
            // inclui o usuário admin ao perfil admin
            _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();

            // inclui as claims do usuário admin
            var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }).Result;
        }

        // se o usuário client não exisitir cria o usuário, define a senha e atribui ao perfil
        if (_userManager.FindByEmailAsync("client1@com.br").Result == null)
        {
            // define os dados do usuário client
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "client1",
                NormalizedUserName = "CLIENT1",
                Email = "client1@com.br",
                NormalizedEmail = "CLIENT1@COM.BR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (11) 12345-6789",
                FirstName = "Usuario",
                LastName = "Client1",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // cria o usuário client e atribui a senha
            IdentityResult resultClient = _userManager.CreateAsync(client, "Client#2026").Result;

            // inclui o usuário client ao perfil client
            if (resultClient.Succeeded)
            {
                _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).Wait();

                // adiciona as claims do usuário client
                var clientClaims = _userManager.AddClaimsAsync(client, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, $"{client.FirstName}"),
                    new Claim(JwtClaimTypes.FamilyName, $"{client.LastName}"),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
                }).Result;
            }
        }
    }
}