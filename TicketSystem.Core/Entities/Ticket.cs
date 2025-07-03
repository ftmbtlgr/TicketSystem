using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TicketSystem.Core.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; } 
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Status { get; set; } // Open, In Progress, Closed, Reopened
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime? DueDate { get; set; } 


        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        [ForeignKey("AssignedToUser")]
        public int? AssignedToUserId { get; set; } 

        // Navigasyon Özellikleri
        public User CreatedByUser { get; set; } = null!; // Zorunlu ilişki, ancak EF Core'un kendisi yükleyeceği için constructor'da null olabilir
        public User? AssignedToUser { get; set; } 
        public ICollection<Comment> Comments { get; set; } = [];
        public ICollection<Attachment> Attachments { get; set; } = [];
    }
}
