using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace TicketSystem.Core.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; } 
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Status { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime? DueDate { get; set; } 


        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        [ForeignKey("AssignedToUser")]
        public int? AssignedToUserId { get; set; } 


        public User CreatedByUser { get; set; } = null!; 
        public User? AssignedToUser { get; set; } 
        public ICollection<Comment> Comments { get; set; } = [];
        public ICollection<Attachment> Attachments { get; set; } = [];
    }
}
