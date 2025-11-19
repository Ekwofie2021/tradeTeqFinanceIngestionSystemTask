using MediatR;

namespace TradeFinanceIngestionSystem.Application.Commands.DeleteNote
{
    public class DeleteNoteCommand : IRequest<bool>
    {
        public Guid NoteId { get; set; }
    }
}
