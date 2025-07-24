using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Services.Interfaces;

namespace TicketSystem.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");
            return Ok(user);
        }

        // GET: api/users/username/{username}
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");
            return Ok(user);
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUser = await _userService.RegisterUserAsync(request);
                var deneme = CreatedAtAction("" +
                    "POST", createdUser);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isValid = await _userService.ValidateCredentialsAsync(request.Username, request.Password);
            if (!isValid) return Unauthorized("Geçersiz kimlik bilgileri.");
            return Ok("Giriş başarılı.");
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest("ID uyuşmazlığı.");
            }

            if (!ModelState.IsValid)
            {
                // Eğer Password veya ConfirmPassword alanı boş gelirse ve Required değilse, validasyon geçebilir.
                // Sadece güncellenecek alanları içeren bir DTO kullanmak daha iyi olabilir
                // veya boş password durumunu burada da kontrol edebilirsiniz.
                // Ancak UserService zaten boş şifre gelirse kendi validasyonunu yapacaktır.
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.UpdateUserAsync(userDto);
                return NoContent(); // Başarılı güncelleme için 204 NoContent döndürülür
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent(); // Başarılı silme için 204 NoContent döndürülür
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message }); // Kullanıcı bulunamazsa 404 NotFound döndür
            }
        }
    }
    public class LoginRequest
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Şifre gerekli.")]
        public string Password { get; set; } = null!;
    }
}