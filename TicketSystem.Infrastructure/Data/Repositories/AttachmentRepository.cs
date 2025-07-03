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
    public class AttachmentRepository(ApplicationDbContext context) : IAttachmentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Attachment?> GetByIdAsync(int id)
        {
            return await _context.Attachments.FindAsync(id);
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByTicketIdAsync(int ticketId)
        {
            return await _context.Attachments
                                 .Where(a => a.TicketId == ticketId)
                                 .OrderBy(a => a.UploadDate) // Ekleri tarihe göre sırala
                                 .ToListAsync();
        }

        public async Task AddAsync(Attachment attachment)
        {
            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attachment attachment)
        {
            _context.Attachments.Update(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Attachments.AnyAsync(a => a.AttachmentId == id);
        }
    }
}
