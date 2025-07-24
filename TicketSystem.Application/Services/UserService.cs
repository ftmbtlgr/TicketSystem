using System.Security.Cryptography;
using System.Text;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;
using Mapster;

namespace TicketSystem.Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var userEntity = await _userRepository.GetByIdAsync(userId);
            // Maplerken Password ve ConfirmPassword alanları otomatik atlanacaktır (Entity'de karşılıkları yok).
            return userEntity?.Adapt<UserDto>();
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var userEntity = await _userRepository.GetByUsernameAsync(username);
            return userEntity?.Adapt<UserDto>();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var userEntities = await _userRepository.GetAllAsync();
            return userEntities.Adapt<List<UserDto>>();
        }

        // Yeni kullanıcı kaydı
        public async Task<UserRegisterDto> RegisterUserAsync(UserRegisterDto userRegisterDto) 
        {
            if (await _userRepository.UserExistsByUsernameAsync(userRegisterDto.Username))
            {
                throw new ApplicationException($"Kullanıcı adı '{userRegisterDto.Username}' zaten mevcut.");
            }

            if (string.IsNullOrEmpty(userRegisterDto.Password))
            {
                throw new ArgumentException("Kayıt için şifre boş olamaz.");
            }

            if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            {
                throw new ApplicationException("Şifre ve tekrarı uyuşmuyor.");
            }

            CreatePasswordHash(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Mapster, UserDto'dan User entity'sine maplerken ConfirmPassword atlanacaktır.
            // UserId, RegistrationDate, LastLoginDate gibi alanlar DTO'dan gelmeyebilir veya servis tarafından set edilebilir.
            var newUserEntity = userRegisterDto.Adapt<User>();

            newUserEntity.PasswordHash = passwordHash;
            newUserEntity.PasswordSalt = passwordSalt;
            newUserEntity.RegistrationDate = DateTime.UtcNow;
            newUserEntity.Role ??= "User"; 
            newUserEntity.IsActive = true;

            await _userRepository.AddAsync(newUserEntity);
            // Kaydedilen entity'yi tekrar UserDto'ya çevirip döndür (şifre bilgisi içermeden)

            return newUserEntity.Adapt<UserRegisterDto>();
        }

        // Kullanıcı güncelleme - UserDto alıyor
        public async Task UpdateUserAsync(UserDto userDto)
        {
            var existingUserEntity = await _userRepository.GetByIdAsync(userDto.UserId);
            if (existingUserEntity == null)
            {
                throw new ApplicationException($"ID'si {userDto.UserId} olan kullanıcı güncelleme için bulunamadı.");
            }

            var userWithSameUsername = await _userRepository.GetByUsernameAsync(userDto.Username);
            if (userWithSameUsername != null && userWithSameUsername.UserId != userDto.UserId)
            {
                throw new ApplicationException($"Kullanıcı adı '{userDto.Username}' başka bir kullanıcı tarafından alınmış.");
            }

            // Mapster ile DTO'daki güncellenebilir alanları mevcut entity üzerine map'leme
            // Mapster, UserDto'daki Password ve ConfirmPassword alanlarını (User entity'sinde karşılığı olmadığı için)
            // otomatik olarak atlayacaktır. Bu, istenen davranıştır.
            userDto.Adapt(existingUserEntity);

            // Şifre güncelleme: Eğer DTO'da yeni bir şifre sağlandıysa, onu da güncelle
            // Password alanı nullable olduğundan, sadece null değilse şifre güncellenir.
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                existingUserEntity.PasswordHash = passwordHash;
                existingUserEntity.PasswordSalt = passwordSalt;
            }

            await _userRepository.UpdateAsync(existingUserEntity);
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new ApplicationException($"ID'si {userId} olan kullanıcı bulunamadı.");
            }
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var userEntity = await _userRepository.GetByUsernameAsync(username);
            if (userEntity == null || userEntity.PasswordHash == null || userEntity.PasswordSalt == null)
            {
                return false;
            }
            return VerifyPasswordHash(password, userEntity.PasswordHash, userEntity.PasswordSalt);
        }

        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _userRepository.UserExistsByUsernameAsync(username);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
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