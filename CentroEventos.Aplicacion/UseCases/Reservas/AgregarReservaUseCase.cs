using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Reservas
{
    public class ReservaAltaUseCase(
        IRepositorioReserva repoReserva,
        IServicioAutorizacion servicioAutorizacion,
        ValidadorReserva validador
        ) : UseCaseConAutorizacion(servicioAutorizacion)
    {
        public async Task EjecutarAsync(Reserva datosReserva, Guid idUsuario)
        {
            await ValidarAutorizacionAsync(idUsuario,Permiso.EventoAlta);
            await validador.ValidarAsync(datosReserva);
            if (await repoReserva.BuscarPorIdAsync(datosReserva.Id) != null)
                throw new OperacionInvalidaException("Ya existe una reserva con el mismo ID.");
            if (await repoReserva.BuscarPersonaPorReservaAsync(datosReserva.PersonaId))
                throw new DuplicadoException("Ya existe una reserva asociada a esta persona.");
            await repoReserva.AgregarAsync(datosReserva);
            await repoReserva.GuardarCambiosAsync();
        }
    }
}
