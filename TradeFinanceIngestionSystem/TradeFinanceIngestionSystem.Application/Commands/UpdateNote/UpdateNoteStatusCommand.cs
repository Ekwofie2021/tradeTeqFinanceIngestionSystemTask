
using MediatR;

namespace TradeFinanceIngestionSystem.Application.Commands.UpdateNote
{
    public class UpdateNoteStatusCommand : IRequest<Guid>
    {
        public Guid NoteId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
