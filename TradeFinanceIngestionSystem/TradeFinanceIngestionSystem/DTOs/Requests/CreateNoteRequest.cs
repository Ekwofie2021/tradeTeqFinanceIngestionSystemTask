using MediatR;

namespace TradeFinanceIngestionSystem.DTOs.Requests
{
    public class CreateNoteRequest
    {
        public string Currency { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
