using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Services.Interfaces;

namespace TicketSystem.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController(ITicketService ticketService) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;

        // GET: api/tickets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET: api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                    return NotFound($"ID'si {id} olan bilet bulunamadı.");

                return Ok(ticket);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = "Bilet getirilirken bir hata oluştu: " + ex.Message });
            }
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTicket = await _ticketService.CreateTicketAsync(ticketDto);
                return CreatedAtAction(nameof(Get), new { id = createdTicket.TicketId }, createdTicket);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bilet oluşturulurken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // PUT: api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
        {
            if (id != ticketDto.TicketId)
                return BadRequest("Bilet ID uyuşmazlığı. URL'deki ID ile gönderilen biletin ID'si farklı.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _ticketService.UpdateTicketAsync(ticketDto);
                return NoContent(); 
            }
            catch (ApplicationException ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bilet güncellenirken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // DELETE: api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(id);
                return NoContent(); // 204 NoContent
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bilet silinirken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // PATCH: api/tickets/{id}/status ticket status degisikligi
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string newStatus, [FromQuery] int? userId)
        {
            if (string.IsNullOrEmpty(newStatus))
            {
                return BadRequest("Yeni durum boş olamaz.");
            }

            try
            {
                await _ticketService.ChangeTicketStatusAsync(id, newStatus, userId);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bilet durumu değiştirilirken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // PATCH: api/tickets/{id}/assign 
        [HttpPatch("{id}/assign")]
        public async Task<IActionResult> AssignTicket(int id, [FromQuery] int assignedToUserId, [FromQuery] int? userId)
        {
            if (assignedToUserId <= 0) 
            {
                return BadRequest("Atanacak kullanıcı ID'si geçerli değil.");
            }

            try
            {
                await _ticketService.AssignTicketAsync(id, assignedToUserId, userId);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bilet atanırken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // POST: api/tickets/{ticketId}/comments
        [HttpPost("{ticketId}/comments")]
        public async Task<IActionResult> AddComment(int ticketId, [FromBody] CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _ticketService.AddCommentToTicketAsync(ticketId, commentDto);
                return Ok(new { message = "Yorum başarıyla eklendi.", comment = commentDto });
            }
            catch (ApplicationException ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Yorum eklenirken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // POST: api/tickets/{ticketId}/attachments
        [HttpPost("{ticketId}/attachments")]
        public async Task<IActionResult> AddAttachment(int ticketId, [FromBody] AttachmentDto attachmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _ticketService.AddAttachmentToTicketAsync(ticketId, attachmentDto);
                return Ok(new { message = "Ek başarıyla eklendi.", attachment = attachmentDto }); 
            }
            catch (ApplicationException ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ek eklenirken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        // GET: api/tickets/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var tickets = await _ticketService.GetTicketsByUserIdAsync(userId);
                return Ok(tickets);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcıya ait biletler getirilirken beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }
    }
}