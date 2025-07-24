using TicketSystem.Core.Entities;

namespace TicketSystem.Application.DTOs
{
    public class TicketDto
    {
        public int TicketId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public int CreatedByUserId { get; set; }
        public string CreatedByUsername { get; set; } = null!;
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUsername { get; set; }
        public ICollection<CommentDto> Comments { get; set; } = [];
        public ICollection<AttachmentDto> Attachments { get; set; } = [];
    }
}
