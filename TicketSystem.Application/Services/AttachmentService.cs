using Mapster;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Services.Interfaces;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Interfaces;

namespace TicketSystem.Application.Services
{
    public class AttachmentService(
        IAttachmentRepository attachmentRepository,
        ITicketRepository ticketRepository) : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;
        private readonly ITicketRepository _ticketRepository = ticketRepository; // Ekin eklendiği ticket'ın varlığını doğrulamak için

        public async Task<AttachmentDto?> GetAttachmentByIdAsync(int attachmentId)
        {
            var attachmentEntity = await _attachmentRepository.GetByIdAsync(attachmentId);
            return attachmentEntity?.Adapt<AttachmentDto>();
        }

        public async Task<IEnumerable<AttachmentDto>> GetAttachmentsByTicketIdAsync(int ticketId)
        {
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found for attachments.");
            }
            var attachmentEntities = await _attachmentRepository.GetAttachmentsByTicketIdAsync(ticketId);
            return attachmentEntities.Adapt<List<AttachmentDto>>();
        }

        public async Task<AttachmentDto> AddAttachmentAsync(AttachmentDto attachmentDto)
        {
            if (!await _ticketRepository.ExistsAsync(attachmentDto.TicketId))
            {
                throw new ApplicationException($"Ticket with ID {attachmentDto.TicketId} not found for adding attachment.");
            }

            var attachmentEntity = attachmentDto.Adapt<Attachment>();

            attachmentEntity.UploadDate = DateTime.UtcNow; 

            await _attachmentRepository.AddAsync(attachmentEntity);

            // Ticket'ın 'LastUpdatedDate' alanını güncelleme iş mantığı
            var ticket = await _ticketRepository.GetByIdAsync(attachmentDto.TicketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }

            return attachmentEntity.Adapt<AttachmentDto>();
        }

        public async Task UpdateAttachmentAsync(AttachmentDto attachmentDto)
        {
            var existingAttachment = await _attachmentRepository.GetByIdAsync(attachmentDto.AttachmentId);
            if (existingAttachment == null)
            {
                throw new ApplicationException($"Attachment with ID {attachmentDto.AttachmentId} not found.");
            }

            if (existingAttachment.TicketId != attachmentDto.TicketId)
            {
                throw new ApplicationException("Cannot change ticket for an existing attachment.");
            }

            attachmentDto.Adapt(existingAttachment);

            await _attachmentRepository.UpdateAsync(existingAttachment);
        }

        public async Task DeleteAttachmentAsync(int attachmentId)
        {
            if (!await _attachmentRepository.ExistsAsync(attachmentId))
            {
                throw new ApplicationException($"Attachment with ID {attachmentId} not found.");
            }
            //Silme yetkilendirmesi (örn. sadece eki yükleyen veya admin silebilir)

            await _attachmentRepository.DeleteAsync(attachmentId);
        }

        public async Task<bool> AttachmentExistsAsync(int attachmentId)
        {
            return await _attachmentRepository.ExistsAsync(attachmentId);
        }
    }
}
