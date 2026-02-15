using System;
using System.Threading.Tasks;
using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.UseCases.Reservas;
using CentroEventos.Aplicacion.Validators;
using FluentAssertions;
using Moq;
using Xunit;

namespace CentroEventos.Tests
{
    public class ReservaAltaUseCaseTests
    {
        private readonly Mock<IRepositorioReserva> _mockRepoReserva;
        private readonly Mock<IRepositorioPersona> _mockRepoPersona;
        private readonly Mock<IRepositorioEventoDeportivo> _mockRepoEvento;
        private readonly Mock<IServicioAutorizacion> _mockAuth;
        private readonly ValidadorReserva _validador;
        private readonly ReservaAltaUseCase _useCase;

        public ReservaAltaUseCaseTests()
        {
            _mockRepoReserva = new Mock<IRepositorioReserva>();
            _mockRepoPersona = new Mock<IRepositorioPersona>();
            _mockRepoEvento = new Mock<IRepositorioEventoDeportivo>();
            _mockAuth = new Mock<IServicioAutorizacion>();

            _validador = new ValidadorReserva(_mockRepoPersona.Object, _mockRepoEvento.Object, _mockRepoReserva.Object);
            _useCase = new ReservaAltaUseCase(_mockRepoReserva.Object, _mockAuth.Object, _validador);
        }

        [Fact]
        public async Task Ejecutar_DebeAgregarReserva_CuandoDatosValidosYUsuarioTienePermisos()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var eventoId = Guid.NewGuid();
            var personaId = Guid.NewGuid();
            var reserva = new Reserva(personaId, eventoId, DateTime.Now, Estado.Pendiente);
            var evento = new EventoDeportivo("Evento", "Desc", DateTime.Now, 2, 10, Guid.NewGuid()) { Id = eventoId };
            var persona = new Persona("123", "Nom", "Ape", "mail", "tel") { Id = personaId };

            _mockAuth.Setup(a => a.PoseeElPermisoAsync(usuarioId, Permiso.EventoAlta)) // Permiso checked in UseCase
                     .ReturnsAsync(true);

            // Mocks para ValidadorReserva
            _mockRepoPersona.Setup(r => r.BuscarPorIdAsync(personaId)).ReturnsAsync(persona);
            _mockRepoEvento.Setup(r => r.BuscarPorIdAsync(eventoId)).ReturnsAsync(evento);
            _mockRepoReserva.Setup(r => r.ContarPorEventoAsync(eventoId)).ReturnsAsync(0);

            // Mocks para UseCase checks
            _mockRepoReserva.Setup(r => r.BuscarPorIdAsync(reserva.Id)).ReturnsAsync((Reserva?)null);
            _mockRepoReserva.Setup(r => r.BuscarPersonaPorReservaAsync(personaId)).ReturnsAsync(false);

            // Act
            await _useCase.EjecutarAsync(reserva, usuarioId);

            // Assert
            _mockRepoReserva.Verify(r => r.AgregarAsync(reserva), Times.Once);
            _mockRepoReserva.Verify(r => r.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task Ejecutar_DebeLanzarCupoExcedidoException_CuandoEventoEstaLleno()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var eventoId = Guid.NewGuid();
            var personaId = Guid.NewGuid();
            var reserva = new Reserva(personaId, eventoId, DateTime.Now, Estado.Pendiente);
            var cupoMaximo = 5;
            var evento = new EventoDeportivo("Evento", "Desc", DateTime.Now, 2, cupoMaximo, Guid.NewGuid()) { Id = eventoId };
            var persona = new Persona("123", "Nom", "Ape", "mail", "tel") { Id = personaId };

            _mockAuth.Setup(a => a.PoseeElPermisoAsync(usuarioId, Permiso.EventoAlta)).ReturnsAsync(true);

            // Validations
            _mockRepoPersona.Setup(r => r.BuscarPorIdAsync(personaId)).ReturnsAsync(persona);
            _mockRepoEvento.Setup(r => r.BuscarPorIdAsync(eventoId)).ReturnsAsync(evento);
            _mockRepoReserva.Setup(r => r.ContarPorEventoAsync(eventoId)).ReturnsAsync(cupoMaximo); // Lleno

            // Act
            Func<Task> action = async () => await _useCase.EjecutarAsync(reserva, usuarioId);

            // Assert
            await action.Should().ThrowAsync<CupoExcedidoException>()
                .WithMessage("No hay cupo disponible para este evento.");
        }

        [Fact]
        public async Task Ejecutar_DebeLanzarDuplicadoException_CuandoPersonaYaTieneReserva()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var eventoId = Guid.NewGuid();
            var personaId = Guid.NewGuid();
            var reserva = new Reserva(personaId, eventoId, DateTime.Now, Estado.Pendiente);
            var evento = new EventoDeportivo("Evento", "Desc", DateTime.Now, 2, 10, Guid.NewGuid()) { Id = eventoId };
            var persona = new Persona("123", "Nom", "Ape", "mail", "tel") { Id = personaId };

            _mockAuth.Setup(a => a.PoseeElPermisoAsync(usuarioId, Permiso.EventoAlta)).ReturnsAsync(true);

            // Validations
            _mockRepoPersona.Setup(r => r.BuscarPorIdAsync(personaId)).ReturnsAsync(persona);
            _mockRepoEvento.Setup(r => r.BuscarPorIdAsync(eventoId)).ReturnsAsync(evento);
            _mockRepoReserva.Setup(r => r.ContarPorEventoAsync(eventoId)).ReturnsAsync(0);

            // UseCase checks
            _mockRepoReserva.Setup(r => r.BuscarPorIdAsync(reserva.Id)).ReturnsAsync((Reserva?)null);
            _mockRepoReserva.Setup(r => r.BuscarPersonaPorReservaAsync(personaId)).ReturnsAsync(true); // Ya tiene reserva

            // Act
            Func<Task> action = async () => await _useCase.EjecutarAsync(reserva, usuarioId);

            // Assert
            await action.Should().ThrowAsync<DuplicadoException>()
                .WithMessage("Ya existe una reserva asociada a esta persona.");
        }
    }
}
