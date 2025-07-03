using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Core.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public required string CommentText { get; set; }
        public DateTime CommentDate { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        public Ticket Ticket { get; set; } = null!; 
        public User User { get; set; } = null!; 
    }
}
