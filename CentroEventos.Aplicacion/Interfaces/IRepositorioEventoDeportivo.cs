using CentroEventos.Aplicacion.Entities;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IRepositorioEventoDeportivo
    {
        Task<EventoDeportivo?> BuscarPorIdAsync(Guid id);
        Task AgregarAsync(EventoDeportivo evento);
        Task ModificarAsync(EventoDeportivo evento);
        Task EliminarAsync(EventoDeportivo evento);
        Task<List<EventoDeportivo>> ListarAsync();
        Task GuardarCambiosAsync();
        Task<bool> ExisteEventoConResponsableAsync(Guid responsableId);
        Task<EventoDeportivo?> BuscarPorNombreAsync(string nombre);
    }
}
