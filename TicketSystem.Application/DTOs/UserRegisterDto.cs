using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Application.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ila 50 karakter arasında olmalıdır.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "E-posta gerekli.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ad gerekli.")]
        [StringLength(50, ErrorMessage = "Ad 50 karakterden uzun olamaz.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Soyad gerekli.")]
        [StringLength(50, ErrorMessage = "Soyad 50 karakterden uzun olamaz.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Şifre gerekli.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Şifre tekrarı gerekli.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}