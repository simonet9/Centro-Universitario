using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.UseCases.Users
{
    public class EliminarUsuarioUseCase(IRepositorioUsuario repoUsuario, IServicioAutorizacion aut)
        : UseCaseConAutorizacion(aut)
    {
        public async Task EjecutarAsync(Guid idUsuarioAEliminar, Guid idUsuarioEjecutor)
        {
            await ValidarAutorizacionAsync(idUsuarioEjecutor, Permiso.UsuarioBaja);
            var usuario = await repoUsuario.ObtenerPorIdAsync(idUsuarioAEliminar) ?? throw new EntidadNotFoundException("Usuario no encontrado.");
            await repoUsuario.EliminarAsync(usuario);
            await repoUsuario.GuardarCambiosAsync();
        }
    }
}