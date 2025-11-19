using TradeFinanceIngestionSystem.Application.Queries.DTOs;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Application.Mapper
{
    public static class NoteMapper
    {
        public static NoteSummaryDto MapToDto(Note note)
        {
            ArgumentNullException.ThrowIfNull(note);

            var currency = note.Currency.ToString();

            return new NoteSummaryDto
            {
                NoteId = note.Id,
                Status = note.Status.ToString(),
                IssueDate = note.IssueDate,
                FinalMaturityDate = note?.FinalMaturityDate,
                ReferenceNumber = note!.ReferenceNumber,
                TotalPurchaseAmount = Price.Create(note!.TotalPurchaseAmount, currency),
                TotalRepaymentAmount = Price.Create(note.TotalRepaymentAmount, currency)
            };
        }
    }
}
