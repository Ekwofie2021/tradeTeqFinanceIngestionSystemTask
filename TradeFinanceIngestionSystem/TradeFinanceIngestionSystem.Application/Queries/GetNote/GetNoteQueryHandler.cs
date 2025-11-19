using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Mapper;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetNote
{
    public class GetNoteQueryHandler(INoteRepository _noteRepository) : IRequestHandler<GetNoteQuery, NoteSummaryDto>
    {
        public async Task<NoteSummaryDto> Handle(GetNoteQuery request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetNoteByIdAsync(request.NoteId, cancellationToken)
                ?? throw new KeyNotFoundException($"NoteId {request.NoteId} not found");

            return NoteMapper.MapToDto(note);
        }
    }
}
