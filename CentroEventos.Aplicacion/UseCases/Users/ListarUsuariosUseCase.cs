using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Users
{
    public class ListarUsuariosUseCase(IRepositorioUsuario repo)
    {
        public async Task<List<Usuario>> EjecutarAsync()
        {
            var usuarios = await repo.ObtenerTodosAsync();
            return ValidadorListas.ValidarNoVacia(usuarios, "No hay usuarios registrados.");
        }
    }
}