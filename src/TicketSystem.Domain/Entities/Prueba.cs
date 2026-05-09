using System;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Prueba de diagnóstico asociada a un ticket
    /// </summary>
    public class Prueba
    {
        public int Id { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Resultado { get; private set; }
        public string Clave { get; private set; }  // Código empresarial (ej: "7R")
        public string Diagnostico { get; private set; }
        public bool ResuelveProblema { get; private set; }

        // Relaciones
        public int TicketId { get; private set; }
        public Ticket Ticket { get; private set; }

        private Prueba() { }

        public Prueba(int ticketId, string resultado, string clave, string diagnostico, bool resuelveProblema)
        {
            if (ticketId <= 0)
                throw new ArgumentException("El ID del ticket es obligatorio", nameof(ticketId));
            
            if (string.IsNullOrWhiteSpace(resultado))
                throw new ArgumentException("El resultado de la prueba es obligatorio", nameof(resultado));

            TicketId = ticketId;
            Resultado = resultado;
            Clave = clave;
            Diagnostico = diagnostico;
            ResuelveProblema = resuelveProblema;
            Fecha = DateTime.UtcNow;
        }
    }
}