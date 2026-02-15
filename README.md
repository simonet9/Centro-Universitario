# Centro de Eventos Universitarios

Sistema para la gestión integral de eventos deportivos, personas, reservas e identidad de usuarios dentro de un centro universitario. El proyecto adopta principios de arquitectura limpia, desacoplamiento por interfaces y persistencia mediante Entity Framework Core con base de datos SQLite. La interfaz de usuario se implementa con Blazor (Razor Components) y MudBlazor, siguiendo patrones asíncronos y de seguridad robusta.

## Objetivos

- Administrar el ciclo de vida de eventos deportivos: alta, modificación, listado y eliminación bajo reglas de negocio.
- Gestionar personas: alta, modificación, listado y eliminación con validaciones de unicidad.
- Registrar y administrar reservas asociadas a eventos, controlando cupos y estados de asistencia.
- Gestionar usuarios con un modelo de permisos y autorizaciones.
- Proveer una interfaz accesible y coherente para las operaciones del sistema.

## Tecnologías y composición

- .NET 8 (C#)
- Blazor (Razor Components) con MudBlazor
- Entity Framework Core
- SQLite (archivo local)
- Inyección de dependencias (DI) nativa de ASP.NET
- **Testing**: xUnit, Moq, FluentAssertions
- **Seguridad**: PBKDF2 (Hashing), Claims-based Authorization

## Estructura del repositorio

- CentroEventos.sln: solución principal para abrir todo el proyecto.
- CentroEventos.Aplicacion: librería con el núcleo de negocio.
  - Entities: `Persona`, `EventoDeportivo`, `Reserva`, `Usuario`.
  - Enum: enumeraciones de dominio (incluye `Permiso` y estados).
  - Exceptions: excepciones de negocio y autorización.
  - Interfaces: contratos de repositorios y servicios.
  - Service: servicios de autorización y utilitarios de aplicación.
  - UseCases: casos de uso por agregado (Personas, Eventos, Reservas, Usuarios) y utilitarios de autorización.
  - Validators: validadores de entidades y listas.
- CentroEventos.Repositorios: persistencia con EF Core y SQLite.
  - Data: contexto de base de datos y utilitarios de inicialización.
  - Repos: implementaciones concretas de repositorios según interfaces del núcleo.
- CentroEventos.UI: aplicación Blazor (servidor).
  - Components: componentes y páginas de la interfaz.
  - Program.cs: configuración del host, servicios, DI, EF Core, MudBlazor, mapeo de componentes.
  - appsettings.json y appsettings.Development.json: configuración de entorno.
- CentroEventos.Tests: proyecto de pruebas unitarias.
  - Tests de Casos de Uso (Eventos, Reservas) con mocking de repositorios.

## Diseño y aptitudes del sistema

- Arquitectura limpia y separada por capas:
  - Dominio y aplicación libres de dependencias de infraestructura.
  - Persistencia y UI referencian únicamente contratos del núcleo.
- Desacoplamiento por interfaces:
  - Los repositorios (`IRepositorioPersona`, `IRepositorioEventoDeportivo`, `IRepositorioReserva`, `IRepositorioUsuario`) permiten reemplazar la infraestructura sin afectar la lógica.
- Casos de uso explícitos:
  - Orquestan reglas de negocio y validaciones, con autorización previa a las operaciones sensibles.
  - Ejemplos: `AgregarPersonaUseCase`, `ListarEventosConCupoDisponibleUseCase`, `EliminarEventoDeportivoUseCase`, `ReservaAltaUseCase`.
- Modelo de autorización y Seguridad:
  - `UseCaseConAutorizacion` centraliza la verificación de permisos (`Permiso`) mediante `IServicioAutorizacion`.
  - Implementación de `AuthenticationStateProvider` personalizado para gestión de sesión segura.
  - Hashing de contraseñas con **PBKDF2** y salts dinámicos.
- Arquitectura Asíncrona:
  - Uso extensivo de `async/await` en todas las capas (UI, Aplicación, Repositorios) para maximizar la escalabilidad y evitar bloqueos.
- Validaciones de negocio:
  - Validadores específicos por entidad (por ejemplo, `ValidadorPersona`, `ValidadorEventoDeportivo`, `ValidadorReserva`) y utilitarios para listas no vacías.
- Persistencia portable:
  - SQLite local, con inicialización del archivo en `Data/CentroEventos.sqlite` dentro del directorio base de la aplicación.
  - EF Core para consultas, conteos y consistencia de transacciones.
- Interfaz modular:
  - Blazor con MudBlazor para notificaciones, estilos y componentes interactivos.
  - Uso de `<AuthorizeView>` y `CascadingAuthenticationState` para control de acceso en UI.
  - Registro de componentes y servicios en `Program.cs`.

## Reglas de negocio principales

- Un evento no puede exceder su cupo máximo de reservas.
- Una persona no puede reservar dos veces el mismo evento.
- No se pueden modificar eventos pasados.
- Las fechas de inicio no pueden ser anteriores al presente.
- No se puede eliminar un evento con reservas asociadas.
- No se puede eliminar una persona que sea responsable de eventos o tenga reservas asociadas.
- El primer usuario registrado es Administrador (todos los permisos).
- Los usuarios nuevos solo tienen permisos de lectura.

## Configuración y ejecución

Requisitos:
- .NET SDK 8 instalado.

Pasos sugeridos:

1. Clonar el repositorio:
   ```
   git clone https://github.com/simonet9/Centro-Universitario.git
   ```
2. Compilar la solución:
   ```
   dotnet build CentroEventos.sln
   ```
3. Ejecutar pruebas unitarias:
   ```
   dotnet test
   ```
4. Ejecutar la UI (Blazor):
   ```
   dotnet run --project CentroEventos.UI
   ```
   - En el arranque, se crea el directorio `Data` y el archivo `CentroEventos.sqlite` si no existe.
   - Se inicializa el contexto y se aplican las configuraciones de MudBlazor y componentes.

Configuración:
- `Program.cs` configura el contexto EF Core:
  - `UseSqlite($"Data Source={dbPath}")` con `dbPath = <BaseDirectory>/Data/CentroEventos.sqlite`.
- Archivos `appsettings.json` y `appsettings.Development.json` están disponibles para ajustes de entorno.

## Módulos funcionales

- Personas:
  - Alta con validación de DNI y email únicos.
  - Listado con validación de no vacío.
  - Eliminación condicionada a la ausencia de reservas y de rol de responsable en eventos.
- Eventos deportivos:
  - Alta, modificación y listado.
  - Eliminación condicionada a la inexistencia de reservas asociadas.
  - Listado de eventos con cupo disponible y fecha válida.
- Reservas:
  - Alta con validaciones de duplicidad por persona y estado inicial.
  - Modificación y eliminación bajo permisos correspondientes.
- Usuarios:
  - Alta, modificación, listado y eliminación.
  - Autorización basada en `Permiso` aplicada en casos de uso sensibles.

## Calidad y mantenibilidad

- Separación de responsabilidades y dependencia invertida.
- Casos de uso y validadores facilitan auditoría de reglas y su evolución.
- Repositorios encapsulan acceso a datos y permiten cambiar de proveedor de base sin modificar el núcleo.
- Inicialización controlada de la base de datos reduce fricción en despliegues locales.
- Uso de MudBlazor mejora la experiencia de usuario y estandariza la presentación.

## Estructura de permisos

El sistema emplea una enumeración de permisos (`Permiso`) para autorizar operaciones. Ejemplos empleados en los casos de uso:
- `EventoAlta`, `EventoModificacion`, `EventoBaja`.
- `ReservaModificacion`.
- Otros permisos pueden existir y ser verificados según el caso de uso.

## Notas sobre datos y persistencia

- El conteo de reservas por evento se utiliza para validar cupos y bloqueos de eliminación.
- Las fechas futuras son condición para que un evento se considere disponible en listados de disponibilidad.
