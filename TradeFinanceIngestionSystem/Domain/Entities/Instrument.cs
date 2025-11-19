
using TradeFinanceIngestionSystem.Domain.Enums;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Domain.Entities
{
    public class Instrument
    {
        public Guid Id { get; set; }
        public Guid NoteId { get; set; }
        public Type Type { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal RepaymentAmount { get; set; }
        public Currency Currency { get; set; }
    }
}
