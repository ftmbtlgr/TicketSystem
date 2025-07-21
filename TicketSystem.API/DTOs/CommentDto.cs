namespace TicketSystem.API.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime CommentDate { get; set; }

        // Yorumu yapan kullanıcının ID'si ve adı
        public int UserId { get; set; }
        public string Username { get; set; } = null!;

        // Yorumun hangi Ticket'a ait olduğunu belirten TicketId
        public int TicketId { get; set; }

        // Ticket veya User navigasyon özelliklerini buraya eklemiyoruz  çünkü bu DTO genellikle bir TicketDto içinde veya Ticket'a ait yorumlar çekilirken kullanılır.
    }
}
