using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(int userId);
        Task<TicketDto> CreateTicketAsync(TicketDto ticket);
        Task UpdateTicketAsync(TicketDto ticket);
        Task DeleteTicketAsync(int ticketId);
        Task AddAttachmentToTicketAsync(int ticketId, Attachment attachment);
        Task ChangeTicketStatusAsync(int ticketId, string newStatus, int? userId); 
        Task AssignTicketAsync(int ticketId, int assignedToUserId, int? userId); // Ticket atama
        Task AddCommentToTicketAsync(int ticketId, Comment comment); // Yorum ekleme
    }
}
