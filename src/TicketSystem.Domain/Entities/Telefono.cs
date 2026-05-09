using System;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Teléfono asociado a un cliente
    /// </summary>
    public class Telefono
    {
        public int Id { get; private set; }
        public string Numero { get; private set; }
        public string Nombre { get; private set; }
        public string Direccion { get; private set; }
        public string Licencia { get; private set; }
        public string Zona { get; private set; }
        public bool Desconectado { get; private set; }
        public int Extensiones { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaActualizacion { get; private set; }

        // Relaciones
        public int ClienteId { get; private set; }
        public Cliente Cliente { get; private set; }

        private Telefono() { }

        public Telefono(string numero, string nombre, string direccion, int clienteId, string zona = null)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("El número de teléfono es obligatorio", nameof(numero));
            
            if (clienteId <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio", nameof(clienteId));

            Numero = numero;
            Nombre = nombre;
            Direccion = direccion;
            ClienteId = clienteId;
            Zona = zona;
            Desconectado = false;
            Extensiones = 0;
            FechaCreacion = DateTime.UtcNow;
        }

        public void ActualizarDatos(string nombre, string direccion, string zona, int extensiones)
        {
            Nombre = nombre;
            Direccion = direccion;
            Zona = zona;
            Extensiones = extensiones;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void Desconectar()
        {
            Desconectado = true;
            FechaActualizacion = DateTime.UtcNow;
        }
    }
}