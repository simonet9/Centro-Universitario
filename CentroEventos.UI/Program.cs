using CentroEventos.UI.Components;
using CentroEventos.UI.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validators;
using CentroEventos.Aplicacion.Service;
using CentroEventos.Aplicacion.UseCases.Evento;
using CentroEventos.Aplicacion.UseCases.Personas;
using CentroEventos.Aplicacion.UseCases.Reservas;
using CentroEventos.Aplicacion.UseCases.Users;
using CentroEventos.Repositorios.Repos;
using CentroEventos.Repositorios.Data;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);



// Database configuration
// Configura la ruta de la base de datos


// Database configuration
// Configura la ruta de la base de datos
var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
Directory.CreateDirectory(dataDir); // aseguro que exista
var dbPath = Path.Combine(dataDir, "CentroEventos.sqlite");
// Configura el contexto con la cadena de conexión


builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Repositorios
builder.Services.AddScoped<IRepositorioPersona, RepositorioPersona>();
builder.Services.AddScoped<IRepositorioEventoDeportivo, RepositorioEventoDeportivo>();
builder.Services.AddScoped<IRepositorioReserva, RepositorioReserva>();

// Servicios
// Servicios
builder.Services.AddScoped<IServicioAutorizacion, ServicioAutorizacion>();

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Validadores
builder.Services.AddScoped<ValidadorReserva>();

// Casos de uso Personas
builder.Services.AddScoped<AgregarPersonaUseCase>();
builder.Services.AddScoped<ModificarPersonaUseCase>();
builder.Services.AddScoped<ListarPersonasUseCase>();
builder.Services.AddScoped<EliminarPersonaUseCase>();

// Casos de uso Eventos
builder.Services.AddScoped<AgregarEventoDeportivoUseCase>();
builder.Services.AddScoped<ModificarEventoDeportivoUseCase>();
builder.Services.AddScoped<ListarEventosDeportivoUseCase>();
builder.Services.AddScoped<EliminarEventoDeportivoUseCase>();
builder.Services.AddScoped<ListarEventosConCupoDisponibleUseCase>();
builder.Services.AddScoped<ListarAsistenciaAEventoUseCase>();

// Casos de uso Reservas
builder.Services.AddScoped<ReservaAltaUseCase>();
builder.Services.AddScoped<ModificarReservaUseCase>();
builder.Services.AddScoped<ListarReservasUseCase>();
builder.Services.AddScoped<EliminarReservaUseCase>();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    
    config.PopoverOptions.ThrowOnDuplicateProvider = false;
});


// Repositorios de usuarios
builder.Services.AddScoped<RegistrarUsuarioUseCase>();

builder.Services.AddScoped<ModificarUsuarioUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();
builder.Services.AddScoped<EliminarUsuarioUseCase>();

builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    CentroEventosSqlite.Inicializar(scope.ServiceProvider);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();