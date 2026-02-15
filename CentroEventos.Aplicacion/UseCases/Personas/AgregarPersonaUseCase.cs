using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;

namespace CentroEventos.Aplicacion.UseCases.Personas
{
    public class AgregarPersonaUseCase(IRepositorioPersona repo)
    {
        public async Task EjecutarAsync(Persona persona)
        {
            ValidadorPersona.Validar(persona);
            if (await repo.ExisteDniAsync(persona.Dni))
                throw new DuplicadoException("Ya existe una persona con el mismo DNI.");
            if (await repo.ObtenerPorEmailAsync(persona.Email))
                throw new DuplicadoException("Ya existe una persona con el mismo email.");
            await repo.AgregarAsync(persona);
            await repo.GuardarCambiosAsync();
        }
    }
}
