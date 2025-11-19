using Microsoft.EntityFrameworkCore;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Infrastructure.DbContexts;

namespace TradeFinanceIngestionSystem.Infrastructure.Repositories
{
    public class NoteRepository(NoteDataContext _noteDataContext) : INoteRepository
    {
        public Task<Note?> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _noteDataContext.Notes.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Note>> GetNotesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _noteDataContext.Notes
                .OrderBy(x => x.IssueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetNotesCountAsync(CancellationToken cancellationToken)
        {
            return await _noteDataContext.Notes.CountAsync(cancellationToken);
        }

        public async Task SaveNoteAsync(Note note, CancellationToken cancellationToken)
        {
            _noteDataContext.Add(note);
            await _noteDataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateNoteStatus(Note note, CancellationToken cancellationToken)
        {
            _noteDataContext.Update(note);
            await _noteDataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteNoteAsync(Note note, CancellationToken cancellationToken)
        {
            _noteDataContext.Remove(note);

            var changes = await _noteDataContext.SaveChangesAsync(cancellationToken);

            return changes > 0;
        }
    }
}
