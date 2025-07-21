namespace TicketSystem.API.DTOs
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
        public string CreatedByUsername { get; set; } = null!; // Oluşturan kullanıcının adı

        public int? AssignedToUserId { get; set; }
        public string? AssignedToUsername { get; set; } // Atanan kullanıcının adı

        // İlişkili koleksiyonlar için ayrı DTO'lar veya ayrı endpoint'ler düşünülebilir.
        // Örneğin, ticket'a ait yorumları listelemek için CommentsDto kullanabiliriz:
        public ICollection<CommentDto>? Comments { get; set; }
        public ICollection<AttachmentDto>? Attachments { get; set; }
    }
}
