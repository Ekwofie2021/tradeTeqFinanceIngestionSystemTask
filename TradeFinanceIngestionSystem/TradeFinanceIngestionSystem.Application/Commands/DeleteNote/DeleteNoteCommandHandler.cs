using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;

namespace TradeFinanceIngestionSystem.Application.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler(INoteRepository _noteRepository, IInstrumentRepository _instrumentRepository) 
        : IRequestHandler<DeleteNoteCommand, bool>
    {
        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetNoteByIdAsync(request.NoteId, cancellationToken)
                ?? throw new KeyNotFoundException($"The Note with ID: {request.NoteId} cannot be found");

            await _instrumentRepository.DeleteInstrumentsAssociatedToNoteByIdAsync(note.Id, cancellationToken);

            var success = await _noteRepository.DeleteNoteAsync(note, cancellationToken);

            return success;
        }
    }
}
