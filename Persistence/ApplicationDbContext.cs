using Microsoft.EntityFrameworkCore;
using Persistence.Outbox;

namespace Persistence
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    }
}
