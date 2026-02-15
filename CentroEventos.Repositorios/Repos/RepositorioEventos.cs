using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Repositorios.Data;
using Microsoft.EntityFrameworkCore;

namespace CentroEventos.Repositorios.Repos
{
    public class RepositorioEventoDeportivo(MyContext context) : IRepositorioEventoDeportivo
    {
        public async Task AgregarAsync(EventoDeportivo evento) => await context.EventosDeportivos.AddAsync(evento);
        public async Task<EventoDeportivo?> BuscarPorIdAsync(Guid id) => await context.EventosDeportivos.FindAsync(id);
        
        public async Task<EventoDeportivo?> BuscarPorNombreAsync(string nombre) => await context.EventosDeportivos.FirstOrDefaultAsync(e => e.Nombre == nombre);
        
        public Task EliminarAsync(EventoDeportivo evento)
        {
            context.EventosDeportivos.Remove(evento);
            return Task.CompletedTask;
        }

        public async Task<List<EventoDeportivo>> ListarAsync() => await context.EventosDeportivos.ToListAsync();
        public async Task GuardarCambiosAsync() => await context.SaveChangesAsync();
        
        public Task ModificarAsync(EventoDeportivo evento)
        {
            context.EventosDeportivos.Update(evento);
            return Task.CompletedTask;
        }

        public async Task<bool> ExisteEventoConResponsableAsync(Guid responsableId) => await context.EventosDeportivos.AnyAsync(e => e.ResponsableId == responsableId);
    }
}