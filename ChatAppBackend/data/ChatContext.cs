using Microsoft.EntityFrameworkCore;
using ChatAppBackend.Models;

namespace ChatAppBackend.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
    }
}