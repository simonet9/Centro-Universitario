using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Reservas
{
    public class ListarAsistenciaAEventoUseCase(
        IRepositorioEventoDeportivo repoEvento,
        IRepositorioReserva repoReserva,
        IRepositorioPersona repoPersona)
    {
        // ● Debe corroborarse que el estado de la reserva sea Presente a la hora de listar las
        //personas que asistieron al evento.
        public async Task<List<Persona>> EjecutarAsync(Guid eventoId)
        {
            var evento = await ValidarYObtenerEventoAsync(eventoId);
            ValidarFechaEvento(evento);
            
            var reservas = await repoReserva.ListarPorEventoAsync(eventoId);
            var asistentes = await ObtenerAsistentesDeReservasAsync(reservas);
            
            return ValidadorListas.ValidarNoVacia(asistentes,"No hay personas registradas para este evento.");
        }

        private async Task<EventoDeportivo> ValidarYObtenerEventoAsync(Guid eventoId)
        {
            return await repoEvento.BuscarPorIdAsync(eventoId) 
                   ?? throw new EntidadNotFoundException("Evento deportivo no encontrado.");
        }

        private static void ValidarFechaEvento(EventoDeportivo evento)
        {
            if (evento.FechaHoraInicio > DateTime.Now)
            {
                throw new InvalidOperationException("El evento aún no ha comenzado.");
            }
        }

        private async Task<List<Persona>> ObtenerAsistentesDeReservasAsync(IEnumerable<Reserva> reservas)
        {
            var reservaPresente = reservas.Where(r => r.EstadoAsistencia == Estado.Presente);
            var personas = new List<Persona>();
            
            // Sequential await to avoid threading issues with DbContext if it were shared (safe bet)
            foreach(var r in reservaPresente)
            {
                var p = await repoPersona.BuscarPorIdAsync(r.PersonaId);
                if (p != null) personas.Add(p);
            }
            return personas;
        }
    }
}