using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Repositorios.Data;
using Microsoft.EntityFrameworkCore;

namespace CentroEventos.Repositorios.Repos
{
    public class RepositorioReserva(MyContext context) : IRepositorioReserva
    {
        public async Task<bool> BuscarPersonaPorReservaAsync(Guid personaId) => await context.Reservas.AnyAsync(r => r.PersonaId == personaId);

        public async Task AgregarAsync(Reserva reserva) => await context.Reservas.AddAsync(reserva);
        public async Task<Reserva?> BuscarPorIdAsync(Guid id) => await context.Reservas.FindAsync(id);
        
        public Task EliminarAsync(Reserva reserva) 
        {
            context.Reservas.Remove(reserva);
            return Task.CompletedTask;
        }

        public async Task<List<Reserva>> ListarAsync() => await context.Reservas.ToListAsync();
        
        public Task ModificarAsync(Reserva reserva)
        {
             context.Reservas.Update(reserva);
             return Task.CompletedTask;
        }

        public async Task<int> ContarPorEventoAsync(Guid eventoId) => await context.Reservas.CountAsync(r => r.EventoDeportivoId == eventoId);
        public async Task<bool> ExisteReservaAsync(Guid personaId, Guid eventoId) => await context.Reservas.AnyAsync(r => r.PersonaId == personaId && r.EventoDeportivoId == eventoId);
        public async Task GuardarCambiosAsync() =>  await context.SaveChangesAsync();
        public async Task<List<Reserva>> ListarPorEventoAsync(Guid eventoId) => await context.Reservas.Where(r => r.EventoDeportivoId == eventoId).ToListAsync();
        public async Task<List<Reserva>> ListarPorPersonaAsync(Guid personaId) => await context.Reservas.Where(r => r.PersonaId == personaId).ToListAsync();

    }
}