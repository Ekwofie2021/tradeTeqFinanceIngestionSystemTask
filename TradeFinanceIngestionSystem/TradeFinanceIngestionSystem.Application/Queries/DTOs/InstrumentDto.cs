using TradeFinanceIngestionSystem.Domain.ValueObjects;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Application.Queries.DTOs
{
    public class InstrumentDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public Price? PurchaseAmount { get; set; }
        public Price? RepaymentAmount { get; set; }

    }
}
