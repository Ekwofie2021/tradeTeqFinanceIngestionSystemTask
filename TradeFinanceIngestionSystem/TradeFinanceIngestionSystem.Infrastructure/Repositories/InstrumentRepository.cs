using Microsoft.EntityFrameworkCore;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Infrastructure.DbContexts;

namespace TradeFinanceIngestionSystem.Infrastructure.Repositories
{
    public class InstrumentRepository(InstrumentDataContext _instrumentDataContext) : IInstrumentRepository
    {
        public async Task<Instrument?> GetInstrumentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _instrumentDataContext.Instruments.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Instrument>> GetInstrumentsAsync(Guid noteId, CancellationToken cancellationToken)
        {
            return await _instrumentDataContext.Instruments
                .AsNoTracking()
                .Where(x => x.NoteId == noteId)
                .OrderBy(x => x.IssueDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, List<Instrument>>> GetInstrumentsByNoteIdsAsync(IEnumerable<Guid> noteIds, CancellationToken cancellationToken)
        {
            var instruments = await _instrumentDataContext.Instruments
                .AsNoTracking()
                .Where(x => noteIds.Contains(x.NoteId))
                .OrderBy(x => x.IssueDate)
                .ToListAsync(cancellationToken);

            return instruments
                .GroupBy(x => x.NoteId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public async Task<List<Instrument>> GetAllInstrumentsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _instrumentDataContext.Instruments
                .AsNoTracking()
                .OrderBy(x => x.IssueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetInstrumentsCountAsync(CancellationToken cancellationToken)
        {
            return await _instrumentDataContext.Instruments.CountAsync(cancellationToken);
        }

        public async Task SaveInstrumentAsync(Instrument instrument, CancellationToken cancellationToken)
        {
            _instrumentDataContext.Add(instrument);
            await _instrumentDataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteInstrumentAsync(Instrument instrument, CancellationToken cancellationToken)
        {
            _instrumentDataContext.Remove(instrument);

            var changes =  await _instrumentDataContext.SaveChangesAsync(cancellationToken);

            return changes > 0;            
        }

        public async Task<bool> DeleteInstrumentsAssociatedToNoteByIdsAsync(Guid noteId, CancellationToken cancellationToken)
        {
            var instruments = await _instrumentDataContext.Instruments
                .Where(i => i.NoteId == noteId)
                .ToListAsync(cancellationToken);

            if (!instruments.Any())
                return false;

            _instrumentDataContext.RemoveRange(instruments);
            var changes = await _instrumentDataContext.SaveChangesAsync(cancellationToken);
            return changes > 0;
        }


        public async Task<Instrument> UpdateInstrumentAsync(Instrument instrument, CancellationToken cancellationToken)
        {
            var updatedInstrument = _instrumentDataContext.Update(instrument);

            await _instrumentDataContext.SaveChangesAsync(cancellationToken);

            return updatedInstrument.Entity;
        }
    }
}
