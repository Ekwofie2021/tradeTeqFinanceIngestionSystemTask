
using MediatR;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateNote
{
    public class UpdateNoteStatusCommand : IRequest<Guid>
    {
        public Guid NoteId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
