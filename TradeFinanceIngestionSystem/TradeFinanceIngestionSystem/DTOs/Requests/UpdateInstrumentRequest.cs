namespace TradeFinanceIngestionSystem.DTOs.Requests
{
    public class UpdateInstrumentRequest
    {
        public string Type { get; set; } = string.Empty;
        public decimal PurchaseAmount { get; set; }
        public decimal RepaymentAmount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
    }
}
