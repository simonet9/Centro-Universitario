using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Evento
{
    public class ModificarEventoDeportivoUseCase(IRepositorioEventoDeportivo repo, IServicioAutorizacion aut)
    : UseCaseConAutorizacion(aut)
    {
        public async Task EjecutarAsync(EventoDeportivo evento, Guid idUsuario)
        {
            await ValidarAutorizacionAsync(idUsuario, Permiso.EventoModificacion);
            ValidadorEventoDeportivo.Validar(evento);
            await repo.ModificarAsync(evento);
            await repo.GuardarCambiosAsync();
        }
    }
}
