using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Reservas
{
    public class ListarReservasUseCase(
        IRepositorioReserva repoReserva
        )
    {
        public async Task<List<Reserva>> EjecutarAsync()
        {
            var reservas = await repoReserva.ListarAsync();
            return ValidadorListas.ValidarNoVacia(reservas, "No hay reservas registradas.");
        }
    }
}