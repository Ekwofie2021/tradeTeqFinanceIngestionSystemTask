using TradeFinanceIngestionSystem.Domain.Enums;
using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Application.Queries.DTOs
{
    public class NoteDetailDto
    {
        public Guid NoteId { get; set; }
        public int ReferenceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<InstrumentDto>? Instruments { get; set; }
        public Price? TotalPurchaseAmount { get; set; }
        public Price? TotalRepaymentAmount { get; set; }
        public DateTime FinalMaturityDate { get; set; }
    }
}
