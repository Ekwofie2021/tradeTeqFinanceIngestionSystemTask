using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Mapper;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;

namespace TradeFinanceIngestionSystem.Application.Queries.GetNotes
{
    public class GetNotesQueryHandler(INoteRepository _noteRepository,
        IInstrumentRepository _instrumentRepository) : IRequestHandler<GetNotesQuery, PagedResult<NoteDetailDto>>
    {
        public async Task<PagedResult<NoteDetailDto>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            var notes = await _noteRepository.GetNotesAsync(request.PageNumber, request.PageSize, cancellationToken);
            var totalCount = await _noteRepository.GetNotesCountAsync(cancellationToken);

            // Fetch all instruments for all notes in a single query
            var noteIds = notes.Select(n => n.Id).ToList();
            var instrumentsByNoteId = await _instrumentRepository.GetInstrumentsByNoteIdsAsync(noteIds, cancellationToken);

            var noteDtos = new List<NoteDetailDto>(notes.Count);

            foreach (var note in notes)
            {
                var instruments = instrumentsByNoteId.GetValueOrDefault(note.Id, []);
                var noteDto = NoteWithInstrumentMapper.MapToDto(note, instruments);
                noteDtos.Add(noteDto);
            }

            return new PagedResult<NoteDetailDto>
            {
                Items = noteDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
