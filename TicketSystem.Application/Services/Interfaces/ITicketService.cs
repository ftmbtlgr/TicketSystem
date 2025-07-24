using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(int userId);
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task UpdateTicketAsync(TicketDto ticketDto);
        Task DeleteTicketAsync(int ticketId);
        Task AddAttachmentToTicketAsync(int ticketId, AttachmentDto attachmentDto);
        Task ChangeTicketStatusAsync(int ticketId, string newStatus, int? userId); 
        Task AssignTicketAsync(int ticketId, int assignedToUserId, int? userId); 
        Task AddCommentToTicketAsync(int ticketId, CommentDto commentDto);
    }
}
