using AccountingLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountingLedger.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
        public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
