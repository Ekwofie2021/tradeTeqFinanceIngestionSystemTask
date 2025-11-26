using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using TradeFinanceIngestionSystem.Domain.ValueObjects;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateInstrument
{
    public class CreateInstrumentCommandHandler(INoteRepository _noteRepository, IInstrumentRepository _instrumentRepository) : IRequestHandler<CreateInstrumentCommand, Guid>
    {
        public async Task<Guid> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetNoteByIdAsync(request.NoteId, cancellationToken) 
                ?? throw new ArgumentException($"NoteId: {request.NoteId} does not exist");

            if (!Enum.TryParse<Type>(request.Type, true, out var type))
            {
                throw new ArgumentException($"Invalid type: {nameof(request.Type)}");
            }
            
            if (!Enum.TryParse<Currency>(request.Currency, true, out var currency))
            {
                throw new ArgumentException($"Invalid currency: {nameof(request.Currency)}");
            }

            if (note.Currency != currency)
            {
                throw new ArgumentException($"Instrument: {nameof(request.Currency)} currency does not match Note currency");
            }

            var instrument = new Instrument
            {
                Id = Guid.NewGuid(),
                NoteId = note.Id,
                Type = type,
                IssueDate = request.IssueDate,
                MaturityDate = request.MaturityDate,
                Currency = note.Currency,
                PurchaseAmount = Price.Create(request.PurchaseAmount, note.Currency.ToString()).Amount,
                RepaymentAmount = Price.Create(request.RepaymentAmount, note.Currency.ToString()).Amount
            };

            await _instrumentRepository.SaveInstrumentAsync(instrument, cancellationToken);

            return instrument.Id;
        }
    }
}
