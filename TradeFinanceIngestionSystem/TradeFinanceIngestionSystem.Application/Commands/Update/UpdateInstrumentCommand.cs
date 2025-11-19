using MediatR;
using TradeFinanceIngestionSystem.Domain.Entities;

namespace TradeFinanceIngestionSystem.Application.Commands.Update
{
    public class UpdateInstrumentCommand : IRequest<Instrument>
    {
        public Guid InstrumentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal RepaymentAmount { get; set; }
    }
}
