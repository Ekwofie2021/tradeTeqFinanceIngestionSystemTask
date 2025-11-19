using TradeFinanceIngestionSystem.Domain.Entities;

namespace TradeFinanceIngestionSystem.Application.Interfaces
{
    public interface INoteRepository
    {
        Task SaveNoteAsync(Note note, CancellationToken cancellationToken); 
        Task<Note?> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken);       
        Task<List<Note>> GetNotesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> GetNotesCountAsync(CancellationToken cancellationToken);
        Task UpdateNoteStatus(Note note, CancellationToken cancellationToken);
        Task<bool> DeleteNoteAsync(Note note, CancellationToken cancellationToken);
    }
}
