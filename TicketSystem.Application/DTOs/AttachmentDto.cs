namespace TicketSystem.Application.DTOs
{
    public class AttachmentDto
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DateTime UploadDate { get; set; }

        public int UploadedByUserId { get; set; }
        public string UploadedByUsername { get; set; } = null!;

        public int TicketId { get; set; }

        //Ticket veya User navigasyon özelliklerini buraya eklemiyoruz. Döngüsel referansları önlemek için
    }
}
