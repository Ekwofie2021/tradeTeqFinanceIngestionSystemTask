using MediatR;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetInstruments
{
    public class GetInstrumentsQuery : IRequest<PagedResult<InstrumentDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
