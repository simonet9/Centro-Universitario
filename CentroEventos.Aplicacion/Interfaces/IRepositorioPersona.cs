using CentroEventos.Aplicacion.Entities;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IRepositorioPersona
    {
        Task<Persona?> BuscarPorIdAsync(Guid id);
        Task<bool> ExisteDniAsync(string dni, Guid idIgnorado = default);
        Task AgregarAsync(Persona persona);
        Task ModificarAsync(Persona persona);
        Task EliminarAsync(Persona persona);
        Task GuardarCambiosAsync();
        Task<List<Persona>> ListarAsync();
        Task<Persona?> ObtenerPorDocumentoAsync(string documento);
        Task<bool> ObtenerPorEmailAsync(string email);
    }
}
