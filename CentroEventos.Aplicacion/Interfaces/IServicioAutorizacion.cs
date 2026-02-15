using CentroEventos.Aplicacion.Enum;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IServicioAutorizacion
    {
        Task<bool> PoseeElPermisoAsync(Guid idUsuario, Permiso permiso);
    }
}