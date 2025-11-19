using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Application.Queries.DTOs
{
    public class NoteSummaryDto
    {
        public Guid NoteId { get; set; }
        public int ReferenceNumber { get; set; } 
        public DateTime IssueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Price? TotalPurchaseAmount { get; set; }
        public Price? TotalRepaymentAmount { get; set; }
        public DateTime? FinalMaturityDate { get; set; }
    }
}
