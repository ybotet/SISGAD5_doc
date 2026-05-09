using System;
using System.Collections.Generic;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Cliente de la empresa de telecomunicaciones
    /// </summary>
    public class Cliente
    {
        private readonly List<Telefono> _telefonos = new();
        private readonly List<Linea> _lineas = new();

        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string Codigo { get; private set; }
        public string Direccion { get; private set; }
        public string Contacto { get; private set; }
        public string TelefonoContacto { get; private set; }
        public string Email { get; private set; }
        public bool Activo { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaActualizacion { get; private set; }

        // Relaciones
        public IReadOnlyList<Telefono> Telefonos => _telefonos.AsReadOnly();
        public IReadOnlyList<Linea> Lineas => _lineas.AsReadOnly();

        private Cliente() { } // Para EF Core

        public Cliente(string nombre, string codigo, string direccion, string contacto, string telefonoContacto, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del cliente es obligatorio", nameof(nombre));
            
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del cliente es obligatorio", nameof(codigo));

            Nombre = nombre;
            Codigo = codigo;
            Direccion = direccion;
            Contacto = contacto;
            TelefonoContacto = telefonoContacto;
            Email = email;
            Activo = true;
            FechaCreacion = DateTime.UtcNow;
        }

        public void ActualizarDatos(string direccion, string contacto, string telefonoContacto, string email)
        {
            Direccion = direccion;
            Contacto = contacto;
            TelefonoContacto = telefonoContacto;
            Email = email;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void Desactivar()
        {
            Activo = false;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void AgregarTelefono(Telefono telefono)
        {
            if (telefono == null)
                throw new ArgumentNullException(nameof(telefono));
            
            _telefonos.Add(telefono);
        }

        public void AgregarLinea(Linea linea)
        {
            if (linea == null)
                throw new ArgumentNullException(nameof(linea));
            
            _lineas.Add(linea);
        }
    }
}