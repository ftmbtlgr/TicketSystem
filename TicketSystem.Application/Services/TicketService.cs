using TicketSystem.Application.DTOs;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;
using Mapster;

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
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            return ticket?.Adapt<TicketDto>();
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return tickets.Adapt<List<TicketDto>>(); 
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new ApplicationException($"ID'si {userId} olan kullanıcı bulunamadı.");
            }
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(userId);
            return tickets.Adapt<List<TicketDto>>(); 
        }

        public async Task<TicketDto> CreateTicketAsync(TicketDto ticketDto) 
        {
            if (!await _userRepository.ExistsAsync(ticketDto.CreatedByUserId))
            {
                throw new ApplicationException($"Oluşturan kullanıcının ID'si {ticketDto.CreatedByUserId} bulunamadı.");
            }

            if (ticketDto.AssignedToUserId.HasValue && !await _userRepository.ExistsAsync(ticketDto.AssignedToUserId.Value))
            {
                throw new ApplicationException($"Atanan kullanıcının ID'si {ticketDto.AssignedToUserId.Value} bulunamadı.");
            }

            var ticket = ticketDto.Adapt<Ticket>(); 

            ticket.CreatedDate = DateTime.UtcNow;
            ticket.LastUpdatedDate = DateTime.UtcNow;
            ticket.Status = ticket.Status ?? "Open"; 

            await _ticketRepository.AddAsync(ticket);
            return ticket.Adapt<TicketDto>(); 
        }

        public async Task UpdateTicketAsync(TicketDto ticketDto)
        {
            var existingTicket = await _ticketRepository.GetByIdAsync(ticketDto.TicketId);
            if (existingTicket == null)
            {
                throw new ApplicationException($"ID'si {ticketDto.TicketId} olan bilet bulunamadı.");
            }

            if (ticketDto.AssignedToUserId.HasValue && !await _userRepository.ExistsAsync(ticketDto.AssignedToUserId.Value))
            {
                throw new ApplicationException($"Atanan kullanıcının ID'si {ticketDto.AssignedToUserId.Value} bulunamadı.");
            }

            ticketDto.Adapt(existingTicket);

            existingTicket.LastUpdatedDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(existingTicket);
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"ID'si {ticketId} olan bilet bulunamadı.");
            }
            await _ticketRepository.DeleteAsync(ticketId);
        }

        public async Task ChangeTicketStatusAsync(int ticketId, string newStatus, int? userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ApplicationException($"ID'si {ticketId} olan bilet bulunamadı.");
            }
            ticket.Status = newStatus;
            ticket.LastUpdatedDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task AssignTicketAsync(int ticketId, int assignedToUserId, int? userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ApplicationException($"ID'si {ticketId} olan bilet bulunamadı.");
            }
            if (!await _userRepository.ExistsAsync(assignedToUserId))
            {
                throw new ApplicationException($"Atanan kullanıcının ID'si {assignedToUserId} bulunamadı.");
            }
            ticket.AssignedToUserId = assignedToUserId;
            ticket.LastUpdatedDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task AddCommentToTicketAsync(int ticketId, CommentDto commentDto) 
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"ID'si {ticketId} olan bilet bulunamadı.");
            }
            if (!await _userRepository.ExistsAsync(commentDto.UserId))
            {
                throw new ApplicationException($"Yorum yapan kullanıcının ID'si {commentDto.UserId} bulunamadı.");
            }

            var comment = commentDto.Adapt<Comment>(); 
            comment.TicketId = ticketId;
            comment.CommentDate = DateTime.UtcNow;

            await _commentRepository.AddAsync(comment);

            var ticket = await _ticketRepository.GetByIdAsync(ticketId); 
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }
        }

        public async Task AddAttachmentToTicketAsync(int ticketId, AttachmentDto attachmentDto) 
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"ID'si {ticketId} olan bilet bulunamadı.");
            }

            var attachment = attachmentDto.Adapt<Attachment>();
            attachment.TicketId = ticketId;
            attachment.UploadDate = DateTime.UtcNow;

            await _attachmentRepository.AddAsync(attachment);

            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }
        }
    }
}