using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserRegisterDto> RegisterUserAsync(UserRegisterDto userRegisterDto); 
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(int userId);
        Task<bool> ValidateCredentialsAsync(string username, string password); 
        Task<bool> UserExistsByUsernameAsync(string username);
    }
}
