using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;

namespace TicketSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        // GET: api/attachments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachmentById(int id)
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
                return NotFound();

            return Ok(attachment);
        }

        // GET: api/attachments/ticket/{ticketId}
        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetAttachmentsByTicketId(int ticketId)
        {
            var attachments = await _attachmentService.GetAttachmentsByTicketIdAsync(ticketId);
            return Ok(attachments);
        }

        // POST: api/attachments
        [HttpPost]
        public async Task<IActionResult> AddAttachment([FromBody] Attachment attachment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdAttachment = await _attachmentService.AddAttachmentAsync(attachment);
                return CreatedAtAction(nameof(GetAttachmentById), new { id = createdAttachment.AttachmentId }, createdAttachment);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/attachments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttachment(int id, [FromBody] Attachment attachment)
        {
            if (id != attachment.AttachmentId)
                return BadRequest("Attachment ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _attachmentService.UpdateAttachmentAsync(attachment);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/attachments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            if (!await _attachmentService.AttachmentExistsAsync(id))
                return NotFound();

            await _attachmentService.DeleteAttachmentAsync(id);
            return NoContent();
        }
    }
}
