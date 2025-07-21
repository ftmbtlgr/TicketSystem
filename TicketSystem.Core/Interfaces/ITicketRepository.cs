using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Core.Interfaces
{
    public interface ITicketRepository
    {
        Task<TicketDto?> GetByIdAsync(int id);
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(int userId); // Belirli bir kullanıcının ticket'ları
        Task AddAsync(TicketDto ticket);
        Task UpdateAsync(TicketDto ticket);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
