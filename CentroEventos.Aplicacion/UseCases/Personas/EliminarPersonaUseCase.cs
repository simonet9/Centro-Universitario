using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.UseCases.Personas
{
    public class EliminarPersonaUseCase(IRepositorioPersona repo, IRepositorioEventoDeportivo repoEvento, IRepositorioReserva repoReserva)
    {
        public async Task EjecutarAsync(Guid idPersona)
        {
            await ValidarAsociaciones(idPersona);
            var persona  = await repo.BuscarPorIdAsync(idPersona) ?? throw new EntidadNotFoundException("Persona no encontrada.");
            await repo.EliminarAsync(persona);
            await repo.GuardarCambiosAsync();
        }
        private async Task ValidarAsociaciones(Guid idPersona)
        {
            if (await repoEvento.ExisteEventoConResponsableAsync(idPersona))
                throw new OperacionInvalidaException("No se puede eliminar la persona porque es responsable de uno o más eventos deportivos.");

            // Optimization: We could have a "CountPorPersona" or "ExistePorPersona" in RepoReserva, 
            // but ListarPorPersona is what we have.
            var reservas = await repoReserva.ListarPorPersonaAsync(idPersona);
            if (reservas.Count > 0)
                throw new OperacionInvalidaException("No se puede eliminar la persona porque tiene reservas asociadas.");
        } 
    }
}
