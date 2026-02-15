using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.Service
{
    public class ServicioAutorizacion(IRepositorioUsuario repo) : IServicioAutorizacion
    {
        public async Task<bool> PoseeElPermisoAsync(Guid idUsuario, Permiso permiso)
        {
            var usuario = await repo.ObtenerPorIdAsync(idUsuario);
            return usuario != null && usuario.Permisos.Any(p => p == permiso);
        }
    }
}