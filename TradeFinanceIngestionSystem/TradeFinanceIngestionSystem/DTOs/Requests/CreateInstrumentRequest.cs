namespace TradeFinanceIngestionSystem.DTOs.Requests
{
    public class CreateInstrumentRequest
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
