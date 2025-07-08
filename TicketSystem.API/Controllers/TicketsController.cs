using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;

namespace TicketSystem.API.Controllers
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
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket ticket)
        {
            var createdTicket = await _ticketService.CreateTicketAsync(ticket);
            return CreatedAtAction(nameof(Get), new { id = createdTicket.TicketId }, createdTicket);
        }

        // PUT: api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.TicketId)
                return BadRequest("Ticket ID mismatch.");

            await _ticketService.UpdateTicketAsync(ticket);
            return NoContent();
        }

        // DELETE: api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return NoContent();
        }

        // PATCH: api/tickets/{id}/status ticket status degisikligi
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string newStatus, [FromQuery] int? userId)
        {
            await _ticketService.ChangeTicketStatusAsync(id, newStatus, userId);
            return NoContent();
        }

        // PATCH: api/tickets/{id}/assign ticket'ı kullanıcıya assign etme
        [HttpPatch("{id}/assign")]
        public async Task<IActionResult> AssignTicket(int id, [FromQuery] int assignedToUserId, [FromQuery] int? userId)
        {
            await _ticketService.AssignTicketAsync(id, assignedToUserId, userId);
            return NoContent();
        }

        // POST: api/tickets/{ticketId}/comments
        [HttpPost("{ticketId}/comments")]
        public async Task<IActionResult> AddComment(int ticketId, [FromBody] Comment comment)
        {
            await _ticketService.AddCommentToTicketAsync(ticketId, comment);
            return Ok(comment);
        }

        // POST: api/tickets/{ticketId}/attachments
        [HttpPost("{ticketId}/attachments")]
        public async Task<IActionResult> AddAttachment(int ticketId, [FromBody] Attachment attachment)
        {
            await _ticketService.AddAttachmentToTicketAsync(ticketId, attachment);
            return Ok(attachment);
        }

        // GET: api/tickets/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var tickets = await _ticketService.GetTicketsByUserIdAsync(userId);
            return Ok(tickets);
        }
    }
}
