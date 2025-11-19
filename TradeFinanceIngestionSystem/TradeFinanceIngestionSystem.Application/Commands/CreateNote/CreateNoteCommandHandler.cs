using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateNote
{
    public class CreateNoteCommandHandler(INoteRepository _noteRepository)
        : IRequestHandler<CreateNoteCommand, Guid>
    {
        public async Task<Guid> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentException("CreateNoteCommand Cannot be null");
            }

            if (!Enum.TryParse<Currency>(request.Currency, true, out var currency))
            {
                throw new ArgumentException($"Invalid currency: {nameof(request.Currency)}");
            }

            var note = new Note
            {
                Id = Guid.NewGuid(),
                Status = Status.DRAFT,
                IssueDate = request.IssueDate,
                ReferenceNumber = request.ReferenceNumber,
                Currency = currency
            };

            await _noteRepository.SaveNoteAsync(note, cancellationToken);

            return note.Id;
        }
    }
}
