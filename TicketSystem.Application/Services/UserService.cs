using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;

namespace TicketSystem.Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            // İş Kuralı: Kullanıcı adı zaten kullanımda mı?
            if (await _userRepository.UserExistsByUsernameAsync(user.Username))
            {
                throw new ApplicationException($"Username '{user.Username}' already exists.");
            }

            // Güvenlik: Şifreyi hash'le ve Salt oluştur
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RegistrationDate = DateTime.UtcNow; // Kayıt tarihi
            user.Role = user.Role ?? "User"; // Varsayılan rol ataması

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            // İş Kuralı: Güncellenen kullanıcı adı başkası tarafından kullanılıyor mu?
            var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser != null && existingUser.UserId != user.UserId)
            {
                throw new ApplicationException($"Username '{user.Username}' is already taken by another user.");
            }

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            // İş Kuralı: Kullanıcı mevcut mu?
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new ApplicationException($"User with ID {userId} not found.");
            }
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || user.PasswordHash == null || user.PasswordSalt == null)
            {
                return false; // Kullanıcı bulunamadı veya şifre bilgileri eksik
            }

            // Şifreyi doğrula
            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
        }

        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _userRepository.UserExistsByUsernameAsync(username);
        }

        // --- Helper Metotlar ---
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
            return true;
        }
    }
}
