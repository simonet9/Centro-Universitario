using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Repositorios.Data;
using Microsoft.EntityFrameworkCore;

namespace CentroEventos.Repositorios.Repos;

public class RepositorioPersona(MyContext context) : IRepositorioPersona
{
    public async Task<bool> ObtenerPorEmailAsync(string email) => await context.Personas.AnyAsync(p => p.Email == email);
    public async Task AgregarAsync(Persona persona) => await context.Personas.AddAsync(persona);        
    public async Task<Persona?> BuscarPorIdAsync(Guid id) => await context.Personas.FirstOrDefaultAsync(p => p.Id == id);
    public async Task<Persona?> ObtenerPorDocumentoAsync(string documento) => await context.Personas.FirstOrDefaultAsync(p => p.Dni == documento || p.Email == documento);
    
    public Task EliminarAsync(Persona persona)
    {
        context.Personas.Remove(persona);
        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync() => await context.SaveChangesAsync();
    public async Task<bool> ExisteDniAsync(string dni, Guid idIgnorado) => await context.Personas.AnyAsync(p => p.Dni == dni && p.Id != idIgnorado);
    
    // Note: The original generic ExisteEmail wasn't in the interface explicitly but implemented. 
    // The implementation for ObtenerPorEmailAsync handles email check if that's what was intended, 
    // or if "ExisteEmail" was a public method not in interface. 
    // The interface has ObtenerPorEmail returning bool, so I implemented that.
    
    public async Task<List<Persona>> ListarAsync() => await context.Personas.ToListAsync();
    
    public Task ModificarAsync(Persona persona)
    {
        context.Personas.Update(persona);
        return Task.CompletedTask;
    }
}