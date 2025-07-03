using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Core.Interfaces
{
    public interface IAttachmentRepository
    {
        Task<Attachment?> GetByIdAsync(int id);
        Task<IEnumerable<Attachment>> GetAttachmentsByTicketIdAsync(int ticketId);
        Task AddAsync(Attachment attachment);
        Task UpdateAsync(Attachment attachment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
