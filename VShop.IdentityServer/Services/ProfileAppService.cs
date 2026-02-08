using System.Security.Claims;
using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using VShop.IdentityServer.Data;

namespace VShop.IdentityServer.Services;

public class ProfileAppService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

    public ProfileAppService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _claimsFactory = claimsFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        // id do usuário no IS
        string id = context.Subject.GetSubjectId();

        // localiz o usuário pelo id
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        // cria ClaimsPrincipal para o usuário
        ClaimsPrincipal userClaims = await _claimsFactory.CreateAsync(user);

        // define a coleção de claims para o usuário
        // e inclui o sobrenome e o nome do usuário
        List<Claim> claims = userClaims.Claims.ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

        // se o userManager do identity suportar role
        if (_userManager.SupportsUserRole)
        {
            // obtem a lista dos nomes das roles para o usuário
            IList<string> roles = await _userManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                // adiciona a role na claim
                claims.Add(new Claim(JwtClaimTypes.Role, role));

                // se roleManager suportar claims para roles
                if (_roleManager.SupportsRoleClaims)
                {
                    // localiza o perfil
                    IdentityRole identityRole = await _roleManager.FindByNameAsync(role);

                    // inclui o perfil
                    if (identityRole != null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                    }
                }
            }
        }

        // retorna as claims no contexto
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        // obtem o id do usuário do IS
        string userId = context.Subject.GetSubjectId();

        // localiza o usuário
        ApplicationUser user = await _userManager.FindByIdAsync(userId);

        // verifica se esta ativo
        context.IsActive = user != null;
    }
}