using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Enums;

namespace TradeFinanceIngestionSystem.Application.Commands.UpdateNote
{
    public class UpdateNoteStatusCommandHandler(INoteRepository _noteRepository)
        : IRequestHandler<UpdateNoteStatusCommand, Guid>
    {
        public async Task<Guid> Handle(UpdateNoteStatusCommand request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetNoteByIdAsync(request.NoteId, cancellationToken)
                ?? throw new KeyNotFoundException($"InstrumentId {request.NoteId} not found");

            if (!Enum.TryParse<Status>(request.Status, true, out var status))
            {
                throw new ArgumentException($"Invalid status: {nameof(request.Status)}");
            }

            note.Status = status;

            await _noteRepository.UpdateNoteStatus(note, cancellationToken);

            return note.Id;
        }
    }
}
