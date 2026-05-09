using System;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;
using Xunit;

namespace TicketSystem.Tests
{
    public class TicketTests
    {
        [Fact]
        public void CrearTicket_ConDatosValidos_CreaTicketCorrectamente()
        {
            // Arrange
            var descripcion = "Problema con el teléfono";
            var clienteId = 1;

            // Act
            var ticket = new Ticket(descripcion, clienteId);

            // Assert
            Assert.Equal(descripcion, ticket.Descripcion);
            Assert.Equal(clienteId, ticket.ClienteId);
            Assert.Equal(EstadoTicket.Abierta, ticket.Estado);
            Assert.NotNull(ticket.FechaCreacion);
        }

        [Fact]
        public void CrearTicket_SinDescripcion_LanzaExcepcion()
        {
            // Arrange
            var descripcion = "";
            var clienteId = 1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Ticket(descripcion, clienteId));
        }

        [Fact]
        public void CrearTicket_PrioridadInvalida_LanzaExcepcion()
        {
            // Arrange
            var descripcion = "Problema";
            var clienteId = 1;
            var prioridadInvalida = 6;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Ticket(descripcion, clienteId, prioridadInvalida));
        }

        [Fact]
        public void RegistrarPrueba_ConPruebaExitosa_CambiaEstadoAResuelta()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Exitosa", "7R", "Cable dañado", resuelveProblema: true);

            // Act
            ticket.RegistrarPrueba(prueba);

            // Assert
            Assert.Equal(EstadoTicket.Resuelta, ticket.Estado);
            Assert.Contains(prueba, ticket.Pruebas);
        }

        [Fact]
        public void RegistrarPrueba_ConPruebaNoExitosa_CambiaEstadoAProbada()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Fallida", "7R", "Cable dañado", resuelveProblema: false);

            // Act
            ticket.RegistrarPrueba(prueba);

            // Assert
            Assert.Equal(EstadoTicket.Probada, ticket.Estado);
        }

        [Fact]
        public void RegistrarPrueba_EnEstadoIncorrecto_LanzaExcepcion()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Exitosa", "7R", "Diagnóstico", true);
            
            // Llevar el ticket a estado Resuelta (vía prueba exitosa)
            ticket.RegistrarPrueba(prueba); // Estado = Resuelta
            
            // Cerrar el ticket (ahora sí se puede)
            ticket.Cerrar(); // Estado = Cerrada
            
            // Intentar registrar una nueva prueba en un ticket cerrado
            var nuevaPrueba = new Prueba(1, "Exitosa", "7R", "Diagnóstico", true);
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ticket.RegistrarPrueba(nuevaPrueba));
        }

        [Fact]
        public void AsignarTrabajador_EnEstadoProbada_CambiaEstadoAAsignada()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Fallida", "7R", "Diagnóstico", false);
            ticket.RegistrarPrueba(prueba); // Estado = Probada

            // Act
            ticket.AsignarTrabajador();

            // Assert
            Assert.Equal(EstadoTicket.Asignada, ticket.Estado);
        }

        [Fact]
        public void AsignarTrabajador_EnEstadoIncorrecto_LanzaExcepcion()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ticket.AsignarTrabajador());
        }

        [Fact]
        public void RegistrarTrabajo_Exitoso_CambiaEstadoAResuelta()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Fallida", "7R", "Diagnóstico", false);
            ticket.RegistrarPrueba(prueba);
            ticket.AsignarTrabajador();
            var trabajo = new Trabajo(1, "Cambiar cable", "7R", esPendiente: false, tiempoMinutos: 30);

            // Act
            ticket.RegistrarTrabajo(trabajo);

            // Assert
            Assert.Equal(EstadoTicket.Resuelta, ticket.Estado);
            Assert.Contains(trabajo, ticket.Trabajos);
        }

        [Fact]
        public void RegistrarTrabajo_Pendiente_CambiaEstadoAPendiente()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Fallida", "7R", "Diagnóstico", false);
            ticket.RegistrarPrueba(prueba);
            ticket.AsignarTrabajador();
            var trabajo = new Trabajo(1, "Cambiar cable", "7R", esPendiente: true);

            // Act
            ticket.RegistrarTrabajo(trabajo);

            // Assert
            Assert.Equal(EstadoTicket.Pendiente, ticket.Estado);
        }

        [Fact]
        public void ResolverPendiente_CambiaEstadoAResuelta()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Fallida", "7R", "Diagnóstico", false);
            ticket.RegistrarPrueba(prueba);
            ticket.AsignarTrabajador();
            var trabajo = new Trabajo(1, "Cambiar cable", "7R", esPendiente: true);
            ticket.RegistrarTrabajo(trabajo); // Estado = Pendiente

            // Act
            ticket.ResolverPendiente();

            // Assert
            Assert.Equal(EstadoTicket.Resuelta, ticket.Estado);
        }

        [Fact]
        public void CerrarTicket_EnEstadoResuelta_CambiaEstadoACerrada()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Exitosa", "7R", "Diagnóstico", true);
            ticket.RegistrarPrueba(prueba); // Estado = Resuelta

            // Act
            ticket.Cerrar();

            // Assert
            Assert.Equal(EstadoTicket.Cerrada, ticket.Estado);
            Assert.NotNull(ticket.FechaCierre);
        }

        [Fact]
        public void CerrarTicket_EnEstadoIncorrecto_LanzaExcepcion()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ticket.Cerrar());
        }

        [Fact]
        public void TiempoResolucionHoras_TicketCerrado_DevuelveTiempo()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);
            var prueba = new Prueba(1, "Exitosa", "7R", "Diagnóstico", true);
            ticket.RegistrarPrueba(prueba);
            ticket.Cerrar();

            // Act
            var tiempo = ticket.TiempoResolucionHoras();

            // Assert
            Assert.NotNull(tiempo);
            Assert.True(tiempo >= 0);
        }

        [Fact]
        public void TiempoResolucionHoras_TicketNoCerrado_DevuelveNull()
        {
            // Arrange
            var ticket = new Ticket("Problema", 1);

            // Act
            var tiempo = ticket.TiempoResolucionHoras();

            // Assert
            Assert.Null(tiempo);
        }
    }
}