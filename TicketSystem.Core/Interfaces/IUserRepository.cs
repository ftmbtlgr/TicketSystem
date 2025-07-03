using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username); // Kullanıcı adı ile giriş için gerekli olabilir
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id); // Kullanıcının var olup olmadığını kontrol etmek için
        Task<bool> UserExistsByUsernameAsync(string username); // Kullanıcı adının benzersizliğini kontrol etmek için
    }
}
