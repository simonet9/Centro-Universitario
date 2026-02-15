using CentroEventos.Aplicacion.Entities;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IRepositorioReserva
    {
        Task<Reserva?> BuscarPorIdAsync(Guid id);
        Task AgregarAsync(Reserva reserva);
        Task ModificarAsync(Reserva reserva);
        Task EliminarAsync(Reserva reserva);
        Task<List<Reserva>> ListarAsync();
        Task<int> ContarPorEventoAsync(Guid eventoId);
        Task<bool> ExisteReservaAsync(Guid personaId, Guid eventoId);
        Task GuardarCambiosAsync();
        Task<List<Reserva>> ListarPorEventoAsync(Guid eventoId);
        Task<List<Reserva>> ListarPorPersonaAsync(Guid personaId);
        Task<bool> BuscarPersonaPorReservaAsync(Guid personaId);
    }
}