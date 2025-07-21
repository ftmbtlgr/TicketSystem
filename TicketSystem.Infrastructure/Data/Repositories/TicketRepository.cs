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
    public class TicketRepository(ApplicationDbContext context) : ITicketRepository
    {
        private readonly ApplicationDbContext _context = context;

        // Belirli bir ID'ye sahip ticket'ı asenkron olarak getirir.
        // İlişkili kullanıcıları ve yorumları da (eager loading) dahil eder.
        public async Task<TicketDto?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                                 .Include(t => t.CreatedByUser)      // Ticket'ı oluşturan kullanıcıyı dahil et
                                 .Include(t => t.AssignedToUser)     // Ticket'ın atandığı kullanıcıyı dahil et
                                 .Include(t => t.Comments)           // Ticket'ın yorumlarını dahil et
                                     .ThenInclude(c => c.User)       // Yorumu yapan kullanıcıyı da dahil et
                                 .Include(t => t.Attachments)        // Ticket'ın eklerini dahil et
                                 .FirstOrDefaultAsync(t => t.TicketId == id);
        }

        // Tüm ticket'ları asenkron olarak getirir (basit listeleme için).
        // Bu metodda daha az "Include" yaparak performans artışı sağlanabilir.
        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            return await _context.Tickets
                                 .Include(t => t.CreatedByUser)
                                 .Include(t => t.AssignedToUser)
                                 .OrderByDescending(t => t.CreatedDate) // En yeni ticket'lar başta
                                 .ToListAsync();
        }

        // Belirli bir kullanıcının oluşturduğu tüm ticket'ları asenkron olarak getirir.
        public async Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(int userId)
        {
            return await _context.Tickets
                                 .Where(t => t.CreatedByUserId == userId || t.AssignedToUserId == userId)
                                 .Include(t => t.CreatedByUser)
                                 .Include(t => t.AssignedToUser)
                                 .OrderByDescending(t => t.CreatedDate)
                                 .ToListAsync();
        }

        // Yeni bir ticket ekler ve değişiklikleri veritabanına kaydeder.
        public async Task AddAsync(TicketDto ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        // Mevcut bir ticket'ın bilgilerini günceller.
        public async Task UpdateAsync(TicketDto ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        // Belirli bir ID'ye sahip ticket'ı siler.
        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        // Belirli bir ID'ye sahip ticket'ın var olup olmadığını kontrol eder.
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.TicketId == id);
        }
    }
}
