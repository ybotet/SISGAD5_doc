using System;

namespace TicketSystem.Domain.Entities
{
    /// <summary>
    /// Material utilizado en reparaciones
    /// </summary>
    public class Material
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public int Stock { get; private set; }
        public string Unidad { get; private set; }
        public bool Activo { get; private set; }

        private Material() { }

        public Material(string codigo, string nombre, string unidad, int stockInicial = 0)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del material es obligatorio", nameof(codigo));
            
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del material es obligatorio", nameof(nombre));

            Codigo = codigo;
            Nombre = nombre;
            Unidad = unidad;
            Stock = stockInicial >= 0 ? stockInicial : 0;
            Activo = true;
        }

        public void ActualizarStock(int cantidad)
        {
            var nuevoStock = Stock + cantidad;
            if (nuevoStock < 0)
                throw new InvalidOperationException($"No hay suficiente stock. Stock actual: {Stock}, cantidad solicitada: {-cantidad}");
            
            Stock = nuevoStock;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public bool HayStock(int cantidadRequerida)
        {
            return Stock >= cantidadRequerida;
        }
    }
}