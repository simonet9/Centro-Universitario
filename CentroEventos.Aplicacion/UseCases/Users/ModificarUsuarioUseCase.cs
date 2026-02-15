using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;
using CentroEventos.Aplicacion.Entities;

namespace CentroEventos.Aplicacion.UseCases.Users;

public class ModificarUsuarioUseCase(
    IRepositorioUsuario repoUsuario,
    IServicioAutorizacion auth)
    : UseCaseConAutorizacion(auth)
{
    public async Task EjecutarAsync(Usuario usuario, Guid usuarioModificadorId)
    {
        await ValidarAutorizacionAsync(usuarioModificadorId, Permiso.UsuarioModificacion);
        ValidadorUsuario.Validar(usuario.Nombre, usuario.Apellido, usuario.HashPassword, usuario.Email);
        await repoUsuario.ActualizarAsync(usuario);
        await repoUsuario.GuardarCambiosAsync();
    }
}