using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Personas
{
    public class ListarPersonasUseCase(IRepositorioPersona repo)
    {
        public async Task<List<Persona>> EjecutarAsync()
        {
            var personas = await repo.ListarAsync();
            return ValidadorListas.ValidarNoVacia(personas, "No hay personas registradas.");
        }
    }
}
