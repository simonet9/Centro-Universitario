using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Evento
{
    public class ListarEventosConCupoDisponibleUseCase(
        IRepositorioEventoDeportivo repoEvento,
        IRepositorioReserva repoReserva)
    {
        public async Task<List<EventoDeportivo>> EjecutarAsync()
        {
            var todosLosEventos = await repoEvento.ListarAsync();
            var eventosDisponibles = new List<EventoDeportivo>();

            foreach(var evento in todosLosEventos)
            {
                 if(await EsEventoDisponibleAsync(evento))
                 {
                     eventosDisponibles.Add(evento);
                 }
            }
            
            return ValidadorListas.ValidarNoVacia(
                eventosDisponibles, 
                "No hay eventos disponibles con cupo.");
        }

        private async Task<bool> EsEventoDisponibleAsync(EventoDeportivo evento)
        {
            return TieneFechaValida(evento) && await TieneCupoDisponibleAsync(evento);
        }

        private static bool TieneFechaValida(EventoDeportivo evento)
        {
            return evento.FechaHoraInicio > DateTime.Now;
        }

        private async Task<bool> TieneCupoDisponibleAsync(EventoDeportivo evento)
        {
            var reservasActuales = await repoReserva.ContarPorEventoAsync(evento.Id);
            return reservasActuales < evento.CupoMaximo;
        }

    }
}