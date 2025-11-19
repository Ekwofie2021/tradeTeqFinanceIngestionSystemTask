
using MediatR;
using TradeFinanceIngestionSystem.Domain.Entities;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<Guid>
    {
        public DateTime IssueDate { get; set; }
        public int ReferenceNumber { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
