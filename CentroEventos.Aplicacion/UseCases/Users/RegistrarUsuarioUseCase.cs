using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;
using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Helpers;

namespace CentroEventos.Aplicacion.UseCases.Users;

public class RegistrarUsuarioUseCase(IRepositorioUsuario repoUsuario)
{
    public async Task EjecutarAsync(string nombre, string apellido, string email, string password, List<Permiso> datosPermisos)
    {
        try
        {
            ValidadorUsuario.Validar(nombre, apellido, password, email);

            if (await repoUsuario.ObtenerPorEmailAsync(email) != null)
            {
                throw new DuplicadoException("Ya existe un usuario con ese email");
            }
            
            // HashHelper is synchronous (CPU bound), that's fine.
            var hashPassword = HashHelper.CalcularHash(password);
            var usuario = new Usuario(nombre, apellido, email, hashPassword, datosPermisos);

            if (!await repoUsuario.ExisteAlgunoAsync())
            {
                foreach (var p in System.Enum.GetValues<Permiso>())
                {
                    usuario.Permisos.Add(p);
                }
            }

            await repoUsuario.AgregarAsync(usuario);
            await repoUsuario.GuardarCambiosAsync();
        }
        catch (Exception ex)
        {
            throw new OperacionInvalidaException(ex.Message);
        }
    }
}