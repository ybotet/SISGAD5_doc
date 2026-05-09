using System;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Línea de telecomunicaciones
    /// </summary>
    public class Linea
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string Codificacion { get; private set; }
        public string Desde { get; private set; }
        public string Hasta { get; private set; }
        public bool Desconectada { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaActualizacion { get; private set; }

        // Relaciones
        public int ClienteId { get; private set; }
        public Cliente Cliente { get; private set; }

        private Linea() { }

        public Linea(string codigo, string desde, string hasta, int clienteId, string codificacion = null)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código de línea es obligatorio", nameof(codigo));
            
            if (clienteId <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio", nameof(clienteId));

            Codigo = codigo;
            Desde = desde;
            Hasta = hasta;
            ClienteId = clienteId;
            Codificacion = codificacion;
            Desconectada = false;
            FechaCreacion = DateTime.UtcNow;
        }

        public void Desconectar()
        {
            Desconectada = true;
            FechaActualizacion = DateTime.UtcNow;
        }
    }
}