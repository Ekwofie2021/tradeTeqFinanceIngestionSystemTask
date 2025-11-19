using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;
using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Application.Queries.GetInstruments
{
    public class GetInstrumentsQueryHandler(IInstrumentRepository _instrumentRepository) 
        : IRequestHandler<GetInstrumentsQuery, PagedResult<InstrumentDto>>
    {
        public async Task<PagedResult<InstrumentDto>> Handle(GetInstrumentsQuery request, CancellationToken cancellationToken)
        {
            var instruments = await _instrumentRepository.GetAllInstrumentsAsync(request.PageNumber, request.PageSize, cancellationToken);
            var totalCount = await _instrumentRepository.GetInstrumentsCountAsync(cancellationToken);

            var instrumentDtos = instruments.Select(i => new InstrumentDto
            {
                Id = i.Id,
                Type = i.Type.ToString(),
                IssueDate = i.IssueDate,
                MaturityDate = i.MaturityDate,
                PurchaseAmount = Price.Create(i.PurchaseAmount, i.Currency.ToString()),
                RepaymentAmount = Price.Create(i.RepaymentAmount, i.Currency.ToString())
            }).ToList();

            return new PagedResult<InstrumentDto>
            {
                Items = instrumentDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
