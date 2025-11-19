using MediatR;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetNotes
{
    public class GetNotesQuery : IRequest<PagedResult<NoteDetailDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
