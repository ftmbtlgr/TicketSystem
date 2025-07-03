using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByTicketIdAsync(int ticketId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
