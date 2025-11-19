using Microsoft.EntityFrameworkCore;
using TradeFinanceIngestionSystem.Domain.Entities;

namespace TradeFinanceIngestionSystem.Infrastructure.DbContexts
{
    public class NoteDataContext(DbContextOptions<NoteDataContext> options) : DbContext(options)
    {
        public virtual DbSet<Note> Notes { get; set; }
    }
}
