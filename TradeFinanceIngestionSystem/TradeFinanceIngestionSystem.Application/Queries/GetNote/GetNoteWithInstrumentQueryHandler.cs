using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Mapper;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetNote
{
    public class GetNoteWithInstrumentQueryHandler(INoteRepository _noteRepository,
        IInstrumentRepository _instrumentRepository) : IRequestHandler<GetNoteWithInstrumentQuery, NoteDetailDto>
    {
        public async Task<NoteDetailDto> Handle(GetNoteWithInstrumentQuery request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetNoteByIdAsync(request.NoteId, cancellationToken)
                ?? throw new KeyNotFoundException($"NoteId {request.NoteId} not found");

            var instruments = await _instrumentRepository.GetInstrumentsAsync(note.Id, cancellationToken);

            return NoteWithInstrumentMapper.MapToDto(note, instruments);
        }
    }
}
