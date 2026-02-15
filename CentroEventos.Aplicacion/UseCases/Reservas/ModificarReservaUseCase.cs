using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.UseCases.Reservas
{
    public class ModificarReservaUseCase(
        IRepositorioReserva repoReserva,
        IServicioAutorizacion aut) : UseCaseConAutorizacion(aut)
    {
        public async Task EjecutarAsync(Reserva reserva, Guid idUsuario)
        {
            await ValidarAutorizacionAsync(idUsuario, Permiso.ReservaModificacion);
            await repoReserva.ModificarAsync(reserva);
            await repoReserva.GuardarCambiosAsync();
        }
    }
}