
using TradeFinanceIngestionSystem.Domain.Enums;
using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Domain.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        public int ReferenceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public Status Status { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public decimal TotalRepaymentAmount { get; set; }
        public DateTime? FinalMaturityDate { get; set; }
        public Currency Currency { get; set; }
    }
}
