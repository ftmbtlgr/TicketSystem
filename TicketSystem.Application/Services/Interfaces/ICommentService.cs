using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment?> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetCommentsByTicketIdAsync(int ticketId);
        Task<Comment> AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        Task<bool> CommentExistsAsync(int commentId);
    }
}
