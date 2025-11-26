using TradeFinanceIngestionSystem.Domain.Entities;

namespace TradeFinanceIngestionSystem.Application.Interfaces
{
    public interface IInstrumentRepository
    {
        Task SaveInstrumentAsync(Instrument note, CancellationToken cancellationToken);
        Task<Instrument?> GetInstrumentByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Instrument>>  GetInstrumentsAsync(Guid noteId, CancellationToken cancellationToken);
        Task<List<Instrument>> GetAllInstrumentsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> GetInstrumentsCountAsync(CancellationToken cancellationToken);
        Task<Dictionary<Guid, List<Instrument>>> GetInstrumentsByNoteIdsAsync(IEnumerable<Guid> noteIds, CancellationToken cancellationToken);         
        Task<Instrument> UpdateInstrumentAsync(Instrument instrument, CancellationToken cancellationToken);
        Task<bool> DeleteInstrumentAsync(Instrument instrument, CancellationToken cancellationToken);  
        Task<bool> DeleteInstrumentsAssociatedToNoteByIdAsync(Guid noteId, CancellationToken cancellationToken);
    }
}
