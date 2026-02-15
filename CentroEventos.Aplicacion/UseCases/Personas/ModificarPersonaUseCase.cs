using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Personas
{
    public class ModificarPersonaUseCase(
        IRepositorioPersona repositorioPersona)
    {
        public async Task EjecutarAsync(Persona persona)
        {
            ValidadorPersona.Validar(persona);
            await repositorioPersona.ModificarAsync(persona);
            await repositorioPersona.GuardarCambiosAsync();
        }
    }
}