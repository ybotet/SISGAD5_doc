using System;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Trabajo de reparación asociado a un ticket
    /// </summary>
    public class Trabajo
    {
        public int Id { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Acciones { get; private set; }
        public string Observaciones { get; private set; }
        public int TiempoMinutos { get; private set; }
        public string Clave { get; private set; }  // Código empresarial
        public bool EsPendiente { get; private set; }

        // Relaciones
        public int TicketId { get; private set; }
        public Ticket Ticket { get; private set; }

        private Trabajo() { }

        public Trabajo(int ticketId, string acciones, string clave, bool esPendiente, int tiempoMinutos = 0, string observaciones = null)
        {
            if (ticketId <= 0)
                throw new ArgumentException("El ID del ticket es obligatorio", nameof(ticketId));
            
            if (string.IsNullOrWhiteSpace(acciones))
                throw new ArgumentException("Las acciones realizadas son obligatorias", nameof(acciones));

            TicketId = ticketId;
            Acciones = acciones;
            Clave = clave;
            EsPendiente = esPendiente;
            TiempoMinutos = tiempoMinutos;
            Observaciones = observaciones;
            Fecha = DateTime.UtcNow;
        }
    }
}