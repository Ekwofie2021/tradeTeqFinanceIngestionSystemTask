using MediatR;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateInstrument
{
    public class CreateInstrumentCommand : IRequest<Guid>
    {
        public Guid NoteId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal PurchaseAmount { get; set; }
        public decimal RepaymentAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
    }
}
