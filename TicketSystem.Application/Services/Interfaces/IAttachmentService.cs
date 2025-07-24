using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<AttachmentDto?> GetAttachmentByIdAsync(int attachmentId);
        Task<IEnumerable<AttachmentDto>> GetAttachmentsByTicketIdAsync(int ticketId);
        Task<AttachmentDto> AddAttachmentAsync(AttachmentDto attachment);
        Task UpdateAttachmentAsync(AttachmentDto attachmentDto);
        Task DeleteAttachmentAsync(int attachmentId);
        Task<bool> AttachmentExistsAsync(int attachmentId);
    }
}
