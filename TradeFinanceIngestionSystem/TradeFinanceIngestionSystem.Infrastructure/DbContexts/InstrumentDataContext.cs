using Microsoft.EntityFrameworkCore;
using TradeFinanceIngestionSystem.Domain.Entities;

namespace TradeFinanceIngestionSystem.Infrastructure.DbContexts
{
    public class InstrumentDataContext(DbContextOptions<InstrumentDataContext> options) : DbContext(options)
    {
        public virtual DbSet<Instrument> Instruments { get; set; }
    }
}
