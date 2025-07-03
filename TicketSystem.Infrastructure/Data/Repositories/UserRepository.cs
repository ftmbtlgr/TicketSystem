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
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id); 
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync(); 
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user); // Kullanıcıyı DbSet'e ekler
            await _context.SaveChangesAsync();  
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user); // Kullanıcıyı güncel olarak işaretler
            await _context.SaveChangesAsync();
        }

        // Belirli bir ID'ye sahip kullanıcıyı siler.
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user); // Kullanıcıyı DbSet'ten siler
                await _context.SaveChangesAsync();
            }
        }

        // Belirli bir ID'ye sahip kullanıcının var olup olmadığını kontrol eder.
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.UserId == id);
        }

        // Belirli bir kullanıcı adına sahip kullanıcının var olup olmadığını kontrol eder.
        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
}
