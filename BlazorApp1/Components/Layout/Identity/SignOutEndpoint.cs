
using Application.SI.Extension.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
namespace BlazorApp1.Components.Layout.Identity;

internal static class SignOutEndpoint
{
    // Método de extensión para configurar el endpoint de cierre de sesión
    public static  IEndpointConventionBuilder MapSignOutEnPoint
        (this IEndpointRouteBuilder endpoint)
    {
        // Verifica que el endpoint no sea nulo
        ArgumentNullException.ThrowIfNull(endpoint);

        // Agrupa los endpoints bajo la ruta "/Account"
        var accountGroup = endpoint.MapGroup("/Account");

        // Configura el endpoint POST para el logout
        accountGroup.MapPost("Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
        {
            // Realiza el cierre de sesión
            await signInManager.SignOutAsync();

            // Redirige al usuario a la página de Login
            return TypedResults.LocalRedirect("/Account/Login");
        });

        // Retorna el grupo de rutas para posibles configuraciones adicionales
        return accountGroup;
    }
}

