using MediatR;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetInstrument
{
    public class GetInstrumentQuery : IRequest<InstrumentDto>
    {
        public Guid InstrumentId { get; set; }
    }
}
