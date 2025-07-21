using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;

namespace TicketSystem.Application.Services
{
    public class TicketService(
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IAttachmentRepository attachmentRepository) : ITicketService
    {
        private readonly ITicketRepository _ticketRepository = ticketRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICommentRepository _commentRepository = commentRepository; 
        private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;

        public async Task<TicketDto?> GetTicketByIdAsync(int ticketId)
        {
            // Ticket'ı getirirken yorum ve ekleri de dahil etmeye devam edebilirsiniz,
            // ancak CommentRepository veya AttachmentRepository üzerinden de yorum/ek getirebilirsiniz.
            return await _ticketRepository.GetByIdAsync(ticketId);
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new ApplicationException($"User with ID {userId} not found.");
            }
            return await _ticketRepository.GetTicketsByUserIdAsync(userId);
        }

        public async Task<TicketDto> CreateTicketAsync(TicketDto ticket)
        {
            if (!await _userRepository.ExistsAsync(ticket.CreatedByUserId))
            {
                throw new ApplicationException($"Creator user with ID {ticket.CreatedByUserId} not found.");
            }

            if (ticket.AssignedToUserId.HasValue && !await _userRepository.ExistsAsync(ticket.AssignedToUserId.Value))
            {
                throw new ApplicationException($"Assigned user with ID {ticket.AssignedToUserId.Value} not found.");
            }

            ticket.CreatedDate = DateTime.UtcNow;
            ticket.LastUpdatedDate = DateTime.UtcNow;
            ticket.Status = ticket.Status ?? "Open";

            await _ticketRepository.AddAsync(ticket);
            return ticket;
        }

        public async Task UpdateTicketAsync(TicketDto ticket)
        {
            var existingTicket = await _ticketRepository.GetByIdAsync(ticket.TicketId);
            if (existingTicket == null)
            {
                throw new ApplicationException($"Ticket with ID {ticket.TicketId} not found.");
            }

            if (ticket.AssignedToUserId.HasValue && !await _userRepository.ExistsAsync(ticket.AssignedToUserId.Value))
            {
                throw new ApplicationException($"Assigned user with ID {ticket.AssignedToUserId.Value} not found.");
            }

            ticket.LastUpdatedDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found.");
            }
            // Ticket silindiğinde ilgili yorumları ve ekleri de silmek istiyorsanız
            // Context'te Cascade Delete tanımladıysanız bu otomatik olur.
            // Aksi takdirde, burada manuel olarak CommentRepository ve AttachmentRepository'yi kullanarak silebilirsiniz.
            await _ticketRepository.DeleteAsync(ticketId);
        }

        public async Task ChangeTicketStatusAsync(int ticketId, string newStatus, int? userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found.");
            }
            // Yetkilendirme ve durum geçiş kuralları buraya gelecek
            ticket.Status = newStatus;
            ticket.LastUpdatedDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task AssignTicketAsync(int ticketId, int assignedToUserId, int? userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found.");
            }
            if (!await _userRepository.ExistsAsync(assignedToUserId))
            {
                throw new ApplicationException($"Assigned user with ID {assignedToUserId} not found.");
            }
            // Yetkilendirme kuralları buraya gelecek
            ticket.AssignedToUserId = assignedToUserId;
            ticket.LastUpdatedDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        // YORUM EKLEME METODU ARTIK CommentRepository'yi KULLANACAK
        public async Task AddCommentToTicketAsync(int ticketId, Comment comment)
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found.");
            }
            if (!await _userRepository.ExistsAsync(comment.UserId))
            {
                throw new ApplicationException($"Comment user with ID {comment.UserId} not found.");
            }

            comment.TicketId = ticketId; // Yorumu ilgili ticket'a bağla
            comment.CommentDate = DateTime.UtcNow;

            await _commentRepository.AddAsync(comment); // CommentRepository aracılığıyla ekle

            // Ticket'ın LastUpdatedDate'ini güncelleyebiliriz, çünkü yeni bir yorum geldi
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }
        }

        // Ek Dosya Ekleme
        public async Task AddAttachmentToTicketAsync(int ticketId, Attachment attachment)
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found.");
            }
            // Dosya yolu, boyutu vs. gibi validasyonlar burada yapılabilir

            attachment.TicketId = ticketId; // Eki ilgili ticket'a bağla
            attachment.UploadDate = DateTime.UtcNow;

            await _attachmentRepository.AddAsync(attachment); // AttachmentRepository aracılığıyla ekle

            // Ticket'ın LastUpdatedDate'ini güncelleyebiliriz, çünkü yeni bir ek geldi
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }
        }
    }
}
