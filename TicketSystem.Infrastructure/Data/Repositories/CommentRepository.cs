using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;

namespace TicketSystem.Infrastructure.Data.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                                 .Include(c => c.User) // Yorumu yapan kullanıcıyı da dahil et
                                 .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTicketIdAsync(int ticketId)
        {
            return await _context.Comments
                                 .Where(c => c.TicketId == ticketId)
                                 .Include(c => c.User) // Yorumu yapan kullanıcıyı da dahil et
                                 .OrderBy(c => c.CommentDate) // Yorumları tarihe göre sırala
                                 .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(c => c.CommentId == id);
        }
    }
}
