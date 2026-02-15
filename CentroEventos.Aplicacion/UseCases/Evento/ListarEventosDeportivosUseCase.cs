using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Evento
{
    public class ListarEventosDeportivoUseCase(IRepositorioEventoDeportivo repo)
    {
        public async Task<List<EventoDeportivo>> EjecutarAsync()
        {
            var eventos = await repo.ListarAsync();
            return ValidadorListas.ValidarNoVacia(eventos, "No hay eventos deportivos registrados.");
        }
    }
}
