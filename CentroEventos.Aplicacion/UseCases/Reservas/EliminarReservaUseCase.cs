using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.UseCases.Reservas
{
    public class EliminarReservaUseCase(
        IRepositorioReserva repoReserva,
        IServicioAutorizacion servicioAutorizacion
    ) : UseCaseConAutorizacion(servicioAutorizacion)
    {
        public async Task EjecutarAsync(Guid idReserva, Guid idUsuario)
        {
            await ValidarAutorizacionAsync(idUsuario, Permiso.ReservaBaja);
            var reserva = await repoReserva.BuscarPorIdAsync(idReserva) ?? throw new EntidadNotFoundException("Reserva no encontrada.");
            await repoReserva.EliminarAsync(reserva);
            await repoReserva.GuardarCambiosAsync();
        }
    }
}