using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.UseCases;

public abstract class UseCaseConAutorizacion(IServicioAutorizacion servicioAutorizacion)
{
    protected async Task ValidarAutorizacionAsync(Guid idUsuario, Permiso permiso)
    {
        if (!await servicioAutorizacion.PoseeElPermisoAsync(idUsuario, permiso))
            throw new FalloAutorizacionException();
    }
}