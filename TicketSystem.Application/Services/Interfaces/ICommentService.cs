using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto?> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<CommentDto>> GetCommentsByTicketIdAsync(int ticketId);
        Task<CommentDto> AddCommentAsync(CommentDto commentDto);
        Task UpdateCommentAsync(CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);
        Task<bool> CommentExistsAsync(int commentId);
    }
}
