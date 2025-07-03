using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Core.Entities
{
    public class Attachment
    {
        [Key]
        public int AttachmentId { get; set; } 
        public required string FileName { get; set; }
        public required string FilePath { get; set; } // Dosyanın depolandığı yol
        public DateTime UploadDate { get; set; }


        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        [ForeignKey("User")]
        public int UploadedByUserId { get; set; }

        // Navigasyon Özellikleri
        public Ticket Ticket { get; set; } = null!; 
        public User UploadedByUser { get; set; } = null!; 
    }
}
