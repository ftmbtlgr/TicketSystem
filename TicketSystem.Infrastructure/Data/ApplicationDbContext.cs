using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Core.Entities;

namespace TicketSystem.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        // Veritabanı tablolarınızı temsil eden DbSet'ler
        public DbSet<User> Users { get; set; }
        public DbSet<TicketDto> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kullanıcı ile Ticket arasındaki ilişkileri açıkça belirtme
            modelBuilder.Entity<TicketDto>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Ticket silindiğinde yaratıcısı silinmesin

            modelBuilder.Entity<TicketDto>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedToUserId)
                .IsRequired(false) // Atanan kullanıcı boş olabilir
                .OnDelete(DeleteBehavior.Restrict); // Ticket silindiğinde atanan kişi silinmesin

            // Comment-Ticket, Comment-User, Attachment-Ticket, Attachment-User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Ticket)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TicketId);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.UploadedByUser)
                .WithMany() // Bu ilişki için User modelinde spesifik bir ICollection tanımlamadık
                .HasForeignKey(a => a.UploadedByUserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
