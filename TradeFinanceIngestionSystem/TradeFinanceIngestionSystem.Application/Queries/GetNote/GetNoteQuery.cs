using MediatR;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetNote
{
    public class GetNoteQuery : IRequest<NoteSummaryDto>
    {
        public Guid NoteId { get; set; }
    }
}
