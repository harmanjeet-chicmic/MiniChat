using Microsoft.EntityFrameworkCore;
using MiniChat.Server.Models;

namespace MiniChat.Server.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages => Set<Message>();
    }
}
