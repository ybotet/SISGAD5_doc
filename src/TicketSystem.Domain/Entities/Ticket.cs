using System;
using System.Collections.Generic;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Ticket de incidencia - Entidad principal del sistema
    /// </summary>
    public class Ticket
    {
        private readonly List<Prueba> _pruebas = new();
        private readonly List<Trabajo> _trabajos = new();
        private EstadoTicket _estado;

        public int Id { get; private set; }
        public string Descripcion { get; private set; }
        public int Prioridad { get; private set; } // 1-5
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaCierre { get; private set; }
        public DateTime? FechaActualizacion { get; private set; }

        // Relaciones
        public int ClienteId { get; private set; }
        public Cliente Cliente { get; private set; }
        public int? TelefonoId { get; private set; }
        public Telefono Telefono { get; private set; }
        public int? LineaId { get; private set; }
        public Linea Linea { get; private set; }

        public EstadoTicket Estado 
        { 
            get => _estado;
            private set
            {
                _estado = value;
                FechaActualizacion = DateTime.UtcNow;
            }
        }

        public IReadOnlyList<Prueba> Pruebas => _pruebas.AsReadOnly();
        public IReadOnlyList<Trabajo> Trabajos => _trabajos.AsReadOnly();

        private Ticket() { }

        public Ticket(string descripcion, int clienteId, int prioridad = 3)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción del ticket es obligatoria", nameof(descripcion));
            
            if (clienteId <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio", nameof(clienteId));
            
            if (prioridad < 1 || prioridad > 5)
                throw new ArgumentException("La prioridad debe ser entre 1 y 5", nameof(prioridad));

            Descripcion = descripcion;
            ClienteId = clienteId;
            Prioridad = prioridad;
            FechaCreacion = DateTime.UtcNow;
            Estado = EstadoTicket.Abierta;
        }

        /// <summary>
        /// Registrar una prueba de diagnóstico
        /// </summary>
        public void RegistrarPrueba(Prueba prueba)
        {
            if (prueba == null)
                throw new ArgumentNullException(nameof(prueba));
            
            if (Estado != EstadoTicket.Abierta)
                throw new InvalidOperationException($"No se puede registrar una prueba en estado {Estado}. El ticket debe estar en estado Abierta.");

            _pruebas.Add(prueba);

            // Si la prueba resuelve el problema, el ticket pasa a Resuelta
            if (prueba.ResuelveProblema)
            {
                Estado = EstadoTicket.Resuelta;
            }
            else
            {
                Estado = EstadoTicket.Probada;
            }
        }

        /// <summary>
        /// Asignar un trabajador al ticket
        /// </summary>
        public void AsignarTrabajador()
        {
            if (Estado != EstadoTicket.Probada)
                throw new InvalidOperationException($"No se puede asignar un trabajador en estado {Estado}. El ticket debe estar en estado Probada.");

            Estado = EstadoTicket.Asignada;
        }

        /// <summary>
        /// Registrar un trabajo de reparación
        /// </summary>
        public void RegistrarTrabajo(Trabajo trabajo)
        {
            if (trabajo == null)
                throw new ArgumentNullException(nameof(trabajo));
            
            if (Estado != EstadoTicket.Asignada)
                throw new InvalidOperationException($"No se puede registrar un trabajo en estado {Estado}. El ticket debe estar en estado Asignada.");

            _trabajos.Add(trabajo);

            if (trabajo.EsPendiente)
            {
                Estado = EstadoTicket.Pendiente;
            }
            else
            {
                Estado = EstadoTicket.Resuelta;
            }
        }

        /// <summary>
        /// Resolver una situación pendiente
        /// </summary>
        public void ResolverPendiente()
        {
            if (Estado != EstadoTicket.Pendiente)
                throw new InvalidOperationException($"No se puede resolver un estado pendiente cuando el estado es {Estado}.");

            Estado = EstadoTicket.Resuelta;
        }

        /// <summary>
        /// Cerrar el ticket
        /// </summary>
        public void Cerrar()
        {
            if (Estado != EstadoTicket.Resuelta)
                throw new InvalidOperationException($"No se puede cerrar un ticket en estado {Estado}. El ticket debe estar en estado Resuelta.");

            Estado = EstadoTicket.Cerrada;
            FechaCierre = DateTime.UtcNow;
        }

        /// <summary>
        /// Tiempo total de resolución en horas
        /// </summary>
        public double? TiempoResolucionHoras()
        {
            if (!FechaCierre.HasValue)
                return null;
            
            return (FechaCierre.Value - FechaCreacion).TotalHours;
        }
    }
}