using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Attachment?> GetAttachmentByIdAsync(int attachmentId)
        {
            return await _attachmentRepository.GetByIdAsync(attachmentId);
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByTicketIdAsync(int ticketId)
        {
            // İş Kuralı: İlgili ticket mevcut mu?
            if (!await _ticketRepository.ExistsAsync(ticketId))
            {
                throw new ApplicationException($"Ticket with ID {ticketId} not found for attachments.");
            }
            return await _attachmentRepository.GetAttachmentsByTicketIdAsync(ticketId);
        }

        public async Task<Attachment> AddAttachmentAsync(Attachment attachment)
        {
            // İş Kuralı: Ekin eklendiği ticket'ın var olduğundan emin ol
            if (!await _ticketRepository.ExistsAsync(attachment.TicketId))
            {
                throw new ApplicationException($"Ticket with ID {attachment.TicketId} not found for adding attachment.");
            }

            // İş Kuralı: Dosya boyutu, tipi gibi validasyonlar burada yapılabilir
            // if (attachment.FileSize > MaxAllowedSize) throw new ApplicationException("File too large.");

            attachment.UploadDate = DateTime.UtcNow;

            await _attachmentRepository.AddAsync(attachment);

            // Ticket'ın 'LastUpdatedDate' alanını güncelleme iş mantığı
            var ticket = await _ticketRepository.GetByIdAsync(attachment.TicketId);
            if (ticket != null)
            {
                ticket.LastUpdatedDate = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }

            return attachment;
        }

        public async Task UpdateAttachmentAsync(Attachment attachment)
        {
            // İş Kuralı: Güncellenecek ek mevcut mu?
            var existingAttachment = await _attachmentRepository.GetByIdAsync(attachment.AttachmentId);
            if (existingAttachment == null)
            {
                throw new ApplicationException($"Attachment with ID {attachment.AttachmentId} not found.");
            }

            // İş Kuralı: Ekin ticket'ı veya dosya yolu gibi önemli alanları değiştirilemez (genellikle)
            if (existingAttachment.TicketId != attachment.TicketId)
            {
                throw new ApplicationException("Cannot change ticket for an existing attachment.");
            }

            // Ek dosya içeriğinin güncellenmesi ayrı bir metotla ele alınabilir (örn. stream olarak)
            // Bu metot sadece meta verileri (Filename, FilePath) güncelliyorsa:
            existingAttachment.FileName = attachment.FileName;
            existingAttachment.FilePath = attachment.FilePath; // Dosya yolunun değişmesi nadirdir

            await _attachmentRepository.UpdateAsync(existingAttachment);
        }

        public async Task DeleteAttachmentAsync(int attachmentId)
        {
            // İş Kuralı: Ek mevcut mu?
            if (!await _attachmentRepository.ExistsAsync(attachmentId))
            {
                throw new ApplicationException($"Attachment with ID {attachmentId} not found.");
            }
            // İş Kuralı: Silme yetkilendirmesi (örn. sadece eki yükleyen veya admin silebilir)

            await _attachmentRepository.DeleteAsync(attachmentId);

            // İş Kuralı: Fiziksel dosyayı sunucudan silme
            // string filePath = Path.Combine(_hostingEnvironment.WebRootPath, attachment.FilePath);
            // if (System.IO.File.Exists(filePath)) { System.IO.File.Delete(filePath); }
            // Bu kısım genellikle Infrastructure katmanındaki bir dosya depolama servisi tarafından yönetilir.
        }

        public async Task<bool> AttachmentExistsAsync(int attachmentId)
        {
            return await _attachmentRepository.ExistsAsync(attachmentId);
        }
    }
}
