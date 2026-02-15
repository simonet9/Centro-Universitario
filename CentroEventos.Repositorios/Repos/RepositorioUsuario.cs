using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Repositorios.Data;
using Microsoft.EntityFrameworkCore;

namespace CentroEventos.Repositorios.Repos;

public class RepositorioUsuario(MyContext context) : IRepositorioUsuario
{
    public async Task<Usuario?> ObtenerPorEmailAsync(string email) => await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    public async Task<Usuario?> ObtenerPorIdAsync(Guid id) => await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    public async Task AgregarAsync(Usuario usuario) => await context.Usuarios.AddAsync(usuario);
    
    public Task ActualizarAsync(Usuario usuario) 
    {
        context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task<List<Usuario>> ObtenerTodosAsync() => await context.Usuarios.ToListAsync();
    public async Task<bool> ExisteAlgunoAsync() => await context.Usuarios.AnyAsync();
    public async Task GuardarCambiosAsync() => await context.SaveChangesAsync();
    
    public Task EliminarAsync(Usuario usuario) 
    {
         context.Usuarios.Remove(usuario);
         return Task.CompletedTask;
    }
}