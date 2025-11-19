using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;

namespace TradeFinanceIngestionSystem.Application.Commands.Update
{
    public class UpdateInstrumentCommandHandler(IInstrumentRepository _instrumentRepository) 
       : IRequestHandler<UpdateInstrumentCommand, Instrument>
    {
        public async Task<Instrument> Handle(UpdateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository.GetInstrumentByIdAsync(request.InstrumentId, cancellationToken) 
                ?? throw new KeyNotFoundException($"InstrumentId: {request.InstrumentId} to be updated is not found");

            // Update instrument
            instrument.Type = Enum.Parse<Domain.Enums.Type>(request.Type);
            instrument.IssueDate = request.IssueDate;
            instrument.MaturityDate = request.MaturityDate;
            instrument.PurchaseAmount = request.PurchaseAmount;
            instrument.RepaymentAmount = request.RepaymentAmount;
            
            var updatedInstrument =  await _instrumentRepository.UpdateInstrumentAsync(instrument, cancellationToken);

            return updatedInstrument;
        }
    }   
}
