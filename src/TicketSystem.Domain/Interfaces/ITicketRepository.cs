using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Domain.Interfaces
{
    /// <summary>
    /// Contrato para el repositorio de tickets
    /// </summary>
    public interface ITicketRepository
    {
        Task<Ticket> GetByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByEstadoAsync(Enums.EstadoTicket estado);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}