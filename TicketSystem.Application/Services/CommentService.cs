using Mapster;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;

namespace TicketSystem.Application.Services
{
    public class CommentService(
        ICommentRepository commentRepository,
        ITicketRepository ticketRepository,
        IUserRepository userRepository) : ICommentService
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ITicketRepository _ticketRepository = ticketRepository;
        private readonly IUserRepository _userRepository = userRepository; 

        public async Task<CommentDto?> GetCommentByIdAsync(int commentId)
        {
            var commentEntity = await _commentRepository.GetByIdAsync(commentId);
            return commentEntity?.Adapt<CommentDto>();
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByTicketIdAsync(int ticketId)
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found for comments.");
            }
            var commentEntities = await _commentRepository.GetCommentsByTicketIdAsync(ticketId);
            return commentEntities.Adapt<List<CommentDto>>();
        }

        public async Task<CommentDto> AddCommentAsync(CommentDto commentDto)
        {
            if (!await _ticketRepository.ExistsAsync(commentDto.TicketId))
            {
                throw new ApplicationException($"Ticket with ID {commentDto.TicketId} not found for adding comment.");
            }
            if (!await _userRepository.ExistsAsync(commentDto.UserId))
            {
                throw new ApplicationException($"User with ID {commentDto.UserId} not found for adding comment.");
            }

            var commentEntity = commentDto.Adapt<Comment>();
            commentEntity.CommentDate = DateTime.UtcNow;

            await _commentRepository.AddAsync(commentEntity);

            var ticket = await _ticketRepository.GetByIdAsync(commentDto.TicketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }

            return commentEntity.Adapt<CommentDto>();
        }

        public async Task UpdateCommentAsync(CommentDto commentDto)
        {
            var existingComment = await _commentRepository.GetByIdAsync(commentDto.CommentId);
            if (existingComment == null)
            {
                throw new ApplicationException($"Yorum ID {commentDto.CommentId} bulunamadı.");
            }

            if (existingComment.TicketId != commentDto.TicketId || existingComment.UserId != commentDto.UserId)
            {
                throw new ApplicationException("Yorumun kullanıcı ve yorumu değiştirilemez.");
            }

            existingComment.CommentText = commentDto.CommentText; 

            await _commentRepository.UpdateAsync(existingComment);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            if (!await _commentRepository.ExistsAsync(commentId))
            {
                throw new ApplicationException($"Comment with ID {commentId} not found.");
            }

            await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<bool> CommentExistsAsync(int commentId)
        {
            return await _commentRepository.ExistsAsync(commentId);
        }
    }
}
