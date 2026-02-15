using System;
using System.Threading.Tasks;
using CentroEventos.Aplicacion.Entities;
using CentroEventos.Aplicacion.Enum;
using CentroEventos.Aplicacion.Exceptions;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.UseCases.Evento;
using FluentAssertions;
using Moq;
using Xunit;

namespace CentroEventos.Tests
{
    public class AgregarEventoDeportivoUseCaseTests
    {
        private readonly Mock<IRepositorioEventoDeportivo> _mockRepo;
        private readonly Mock<IServicioAutorizacion> _mockAuth;
        private readonly AgregarEventoDeportivoUseCase _useCase;

        public AgregarEventoDeportivoUseCaseTests()
        {
            _mockRepo = new Mock<IRepositorioEventoDeportivo>();
            _mockAuth = new Mock<IServicioAutorizacion>();
            _useCase = new AgregarEventoDeportivoUseCase(_mockRepo.Object, _mockAuth.Object);
        }

        [Fact]
        public async Task Ejecutar_DebeAgregarEvento_CuandoDatosSonValidosYUsuarioTienePermisos()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var evento = new EventoDeportivo("Torneo", "Futbol 5", DateTime.Now.AddDays(1), 2, 10, Guid.NewGuid());
            
            _mockAuth.Setup(a => a.PoseeElPermisoAsync(usuarioId, Permiso.EventoAlta))
                     .ReturnsAsync(true);

            _mockRepo.Setup(r => r.BuscarPorIdAsync(evento.Id))
                     .ReturnsAsync((EventoDeportivo?)null);

            // Act
            await _useCase.EjecutarAsync(evento, usuarioId);

            // Assert
            _mockRepo.Verify(r => r.AgregarAsync(evento), Times.Once);
            _mockRepo.Verify(r => r.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task Ejecutar_DebeLanzarFalloAutorizacionException_CuandoUsuarioNoTienePermisos()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var evento = new EventoDeportivo("Torneo", "Futbol 5", DateTime.Now.AddDays(1), 2, 10, Guid.NewGuid());

            _mockAuth.Setup(a => a.PoseeElPermisoAsync(usuarioId, Permiso.EventoAlta))
                     .ReturnsAsync(false);

            // Act
            Func<Task> action = async () => await _useCase.EjecutarAsync(evento, usuarioId);

            // Assert
            await action.Should().ThrowAsync<FalloAutorizacionException>();
        }

        [Fact]
        public async Task Ejecutar_DebeLanzarOperacionInvalidaException_CuandoEventoYaExiste()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var evento = new EventoDeportivo("Torneo", "Futbol 5", DateTime.Now.AddDays(1), 2, 10, Guid.NewGuid());

            _mockAuth.Setup(a => a.PoseeElPermisoAsync(usuarioId, Permiso.EventoAlta))
                     .ReturnsAsync(true);

            _mockRepo.Setup(r => r.BuscarPorIdAsync(evento.Id))
                     .ReturnsAsync(evento);

            // Act
            Func<Task> action = async () => await _useCase.EjecutarAsync(evento, usuarioId);

            // Assert
            await action.Should().ThrowAsync<OperacionInvalidaException>()
                .WithMessage("Ya existe un evento deportivo con el mismo ID.");
        }
    }
}
