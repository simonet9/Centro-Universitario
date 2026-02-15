using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Evento
{
    public class AgregarEventoDeportivoUseCase(IRepositorioEventoDeportivo repo, IServicioAutorizacion aut)
    : UseCaseConAutorizacion(aut)
    {
        public async Task EjecutarAsync(EventoDeportivo eventoDeportivo, Guid usuarioId)
        {
            await ValidarAutorizacionAsync(usuarioId, Permiso.EventoAlta);
            ValidadorEventoDeportivo.Validar(eventoDeportivo);
            
            if(await repo.BuscarPorIdAsync(eventoDeportivo.Id) != null)
                throw new OperacionInvalidaException("Ya existe un evento deportivo con el mismo ID.");

            await repo.AgregarAsync(eventoDeportivo);
            await repo.GuardarCambiosAsync();
        }
    }
}
