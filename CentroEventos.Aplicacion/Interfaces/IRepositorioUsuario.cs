using CentroEventos.Aplicacion.Entities;

namespace CentroEventos.Aplicacion.Interfaces;

public interface IRepositorioUsuario
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task<Usuario?> ObtenerPorIdAsync(Guid id);
    Task AgregarAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task<List<Usuario>> ObtenerTodosAsync();
    Task<bool> ExisteAlgunoAsync();
    Task GuardarCambiosAsync();
    Task EliminarAsync(Usuario usuario);
}