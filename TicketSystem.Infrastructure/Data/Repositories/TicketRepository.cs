using Microsoft.EntityFrameworkCore;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;

namespace TicketSystem.Infrastructure.Data.Repositories
{
    public class TicketRepository(ApplicationDbContext context) : ITicketRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                                 .Include(t => t.CreatedByUser)      
                                 .Include(t => t.AssignedToUser)   
                                 .Include(t => t.Comments)           
                                     .ThenInclude(c => c.User)       
                                 .Include(t => t.Attachments)       
                                 .FirstOrDefaultAsync(t => t.TicketId == id);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                                 .Include(t => t.CreatedByUser)
                                 .Include(t => t.AssignedToUser)
                                 .OrderByDescending(t => t.CreatedDate) // En yeni ticket'lar başta
                                 .ToListAsync();
        }
  
        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(int userId)
        {
            return await _context.Tickets
                                 .Where(t => t.CreatedByUserId == userId || t.AssignedToUserId == userId)
                                 .Include(t => t.CreatedByUser)
                                 .Include(t => t.AssignedToUser)
                                 .OrderByDescending(t => t.CreatedDate)
                                 .ToListAsync();
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.TicketId == id);
        }
    }
}
