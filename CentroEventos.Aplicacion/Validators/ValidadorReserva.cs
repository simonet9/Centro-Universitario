using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.Validators
{
    public class ValidadorReserva(
        IRepositorioPersona repoPersona,
        IRepositorioEventoDeportivo repoEvento,
        IRepositorioReserva repoReserva)
    {
    public async Task ValidarAsync(Reserva reserva)
    {
        if (await repoPersona.BuscarPorIdAsync(reserva.PersonaId) == null)
            throw new EntidadNotFoundException("La persona no existe.");

        var evento = await repoEvento.BuscarPorIdAsync(reserva.EventoDeportivoId) ?? throw new EntidadNotFoundException("El evento no existe.");
        var cantidadReservas = await repoReserva.ContarPorEventoAsync(reserva.EventoDeportivoId);
        if (cantidadReservas >= evento.CupoMaximo)
            throw new CupoExcedidoException("No hay cupo disponible para este evento.");
    }
    }
}
