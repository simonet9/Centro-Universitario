using System.Security.Claims;
using CentroEventos.Aplicacion.Entities;
using Microsoft.AspNetCore.Components.Authorization;

namespace CentroEventos.UI.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public void Login(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}")
        };

        foreach (var permiso in usuario.Permisos)
        {
            claims.Add(new Claim(ClaimTypes.Role, permiso.ToString()));
        }

        var identity = new ClaimsIdentity(claims, "CustomAuth");
        _currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void Logout()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
