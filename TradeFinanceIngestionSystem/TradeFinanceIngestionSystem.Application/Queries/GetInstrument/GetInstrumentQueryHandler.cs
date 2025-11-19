using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;
using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Application.Queries.GetInstrument
{
    public class GetInstrumentQueryHandler(IInstrumentRepository _instrumentRepository) 
        : IRequestHandler<GetInstrumentQuery, InstrumentDto>
    {
        public async Task<InstrumentDto> Handle(GetInstrumentQuery request, CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository.GetInstrumentByIdAsync(request.InstrumentId, cancellationToken)
                ?? throw new KeyNotFoundException($"InstrumentId {request.InstrumentId} not found");

            return new InstrumentDto
            {
                Id = instrument.Id,
                Type = instrument.Type.ToString(),
                IssueDate = instrument.IssueDate,
                MaturityDate = instrument.MaturityDate,
                PurchaseAmount = Price.Create(instrument.PurchaseAmount, instrument.Currency.ToString()),
                RepaymentAmount = Price.Create(instrument.RepaymentAmount, instrument.Currency.ToString())
            };
        }
    }
}
