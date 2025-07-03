using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<Attachment?> GetAttachmentByIdAsync(int attachmentId);
        Task<IEnumerable<Attachment>> GetAttachmentsByTicketIdAsync(int ticketId);
        Task<Attachment> AddAttachmentAsync(Attachment attachment);
        Task UpdateAttachmentAsync(Attachment attachment);
        Task DeleteAttachmentAsync(int attachmentId);
        Task<bool> AttachmentExistsAsync(int attachmentId);
    }
}
