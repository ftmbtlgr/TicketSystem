using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TicketSystem.Core.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; } 
        public required string Username { get; set; }
        public byte[] PasswordHash { get; set; } = null!; 
        public byte[] PasswordSalt { get; set; } = null!;
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Role { get; set; } = "User";
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

        // İlişkiler
        public ICollection<Ticket>? CreatedTickets { get; set; } 
        public ICollection<Ticket>? AssignedTickets { get; set; } 
        public ICollection<Comment>? Comments { get; set; } 
    }
}
