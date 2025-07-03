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
    public class CommentService(
        ICommentRepository commentRepository,
        ITicketRepository ticketRepository,
        IUserRepository userRepository) : ICommentService
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ITicketRepository _ticketRepository = ticketRepository; // Yorumun eklendiği ticket'ın varlığını doğrulamak için
        private readonly IUserRepository _userRepository = userRepository;     // Yorumu yapan kullanıcının varlığını doğrulamak için

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await _commentRepository.GetByIdAsync(commentId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTicketIdAsync(int ticketId)
        {
            // İş Kuralı: İlgili ticket mevcut mu?
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found for comments.");
            }
            return await _commentRepository.GetCommentsByTicketIdAsync(ticketId);
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            // İş Kuralı: Yorumun eklendiği ticket'ın var olduğundan emin ol
            if (!await _ticketRepository.ExistsAsync(comment.TicketId))
            {
                throw new ApplicationException($"Ticket with ID {comment.TicketId} not found for adding comment.");
            }

            // İş Kuralı: Yorumu yapan kullanıcının var olduğundan emin ol
            if (!await _userRepository.ExistsAsync(comment.UserId))
            {
                throw new ApplicationException($"User with ID {comment.UserId} not found for adding comment.");
            }

            comment.CommentDate = DateTime.UtcNow;

            await _commentRepository.AddAsync(comment);

            // Ticket'ın 'LastUpdatedDate' alanını güncelleme iş mantığı
            var ticket = await _ticketRepository.GetByIdAsync(comment.TicketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }

            return comment;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            // İş Kuralı: Güncellenecek yorum mevcut mu?
            var existingComment = await _commentRepository.GetByIdAsync(comment.CommentId);
            if (existingComment == null)
            {
                throw new ApplicationException($"Comment with ID {comment.CommentId} not found.");
            }

            // İş Kuralı: Yorumun ticket'ı veya kullanıcısı değiştirilemez (genellikle)
            if (existingComment.TicketId != comment.TicketId || existingComment.UserId != comment.UserId)
            {
                throw new ApplicationException("Cannot change ticket or user for an existing comment.");
            }

            // Yorumun sadece metninin veya diğer güncellenebilir alanlarının değiştiğini varsayıyoruz.
            existingComment.CommentText = comment.CommentText; // Sadece metin güncelleniyorsa

            await _commentRepository.UpdateAsync(existingComment);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            // İş Kuralı: Yorum mevcut mu?
            if (!await _commentRepository.ExistsAsync(commentId))
            {
                throw new ApplicationException($"Comment with ID {commentId} not found.");
            }
            // İş Kuralı: Silme yetkilendirmesi (örn. sadece yorumu yapan veya admin silebilir)

            await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<bool> CommentExistsAsync(int commentId)
        {
            return await _commentRepository.ExistsAsync(commentId);
        }
    }
}
