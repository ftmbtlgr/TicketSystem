using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketSystem.Core.Entities;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorTestController : ControllerBase
    {
        [HttpGet("unauthorized")] //401
        public IActionResult GetUnauthorized()
        {
            return Unauthorized("Bu işlem için yetkiniz yok.");
        }

        [HttpGet("notfound")] //404
        public IActionResult GetNotFound()
        {
            return NotFound("Bilet bulunamadı.");
        }

        [HttpGet("badrequest")] 
        public IActionResult GetBadRequest()
        {
            return BadRequest("Geçersiz istek.");
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("Sunucu tarafında beklenmeyen bir hata oluştu.");
        }

        [HttpPost("validationerror")]   // 400  
        public IActionResult GetValidationError([FromBody] UserDto ticket)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            return Ok("Doğrulama geçti (ama normalde burada hata alınmalı).");
        }

        [Authorize]
        [HttpGet("secret")]     //JWT ile korunan alan
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"Merhaba {name}, gizli alana eriştiniz.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-secret")]   //JWT ile korunan ve sadece adminlere özel alan
        public IActionResult GetAdminSecret()
        {
            return Ok("Yalnızca adminlere özel içerik!");
        }
    }
}
