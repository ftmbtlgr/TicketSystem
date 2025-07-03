using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> RegisterUserAsync(User user, string password); // Kullanıcı kaydı için
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<bool> ValidateCredentialsAsync(string username, string password); // Giriş için
        Task<bool> UserExistsByUsernameAsync(string username);
    }
}
