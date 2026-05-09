namespace TicketSystem.Domain.Enums
{
    /// <summary>
    /// Estados posibles del ticket
    /// </summary>
    public enum EstadoTicket
    {
        /// <summary>Ticket creado, esperando diagnóstico</summary>
        Abierta = 0,
        
        /// <summary>Diagnóstico realizado, requiere reparación</summary>
        Probada = 1,
        
        /// <summary>Técnico asignado a la reparación</summary>
        Asignada = 2,
        
        /// <summary>Reparación pendiente (falta material, etc.)</summary>
        Pendiente = 3,
        
        /// <summary>Problema resuelto</summary>
        Resuelta = 4,
        
        /// <summary>Ticket finalizado</summary>
        Cerrada = 5
    }
}