using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.UseCases.Evento
{
    public class EliminarEventoDeportivoUseCase(IRepositorioEventoDeportivo repositorioEvento, IServicioAutorizacion aut, IRepositorioReserva repoReserva)
    : UseCaseConAutorizacion(aut)
    {
        public async Task EjecutarAsync(Guid eventoId, Guid usuarioId)
        {
            await ValidarAutorizacionAsync(usuarioId, Permiso.EventoBaja);
            await ValidarReservasExistentes(eventoId);
            var evento = await repositorioEvento.BuscarPorIdAsync(eventoId) ?? throw new EntidadNotFoundException("Evento deportivo no encontrado.");
            await repositorioEvento.EliminarAsync(evento);
            await repositorioEvento.GuardarCambiosAsync();
        }
        private async Task ValidarReservasExistentes(Guid eventoId)
        {
            if (await repoReserva.ContarPorEventoAsync(eventoId) > 0)
            {
                throw new OperacionInvalidaException("Existen reservas asociadas a este evento deportivo. No se puede eliminar.");
            }
        }
    }
}
