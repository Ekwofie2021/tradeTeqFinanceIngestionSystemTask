using MediatR;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetNote
{
    public class GetNoteWithInstrumentQuery : IRequest<NoteDetailDto>
    {
        public Guid NoteId { get; set; }
    }
}
