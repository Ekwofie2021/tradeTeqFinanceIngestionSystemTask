using TradeFinanceIngestionSystem.Application.Queries.DTOs;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.ValueObjects;

namespace TradeFinanceIngestionSystem.Application.Mapper
{
    public static class NoteWithInstrumentMapper
    {
        public static NoteDetailDto MapToDto(Note note, IEnumerable<Instrument> instruments)
        {
            var instrumentsList = instruments.ToList();
            var currency = note.Currency.ToString();

            return new NoteDetailDto
            {
                NoteId = note.Id,
                Status = note.Status.ToString(),
                IssueDate = note.IssueDate,
                FinalMaturityDate = instrumentsList.MaxBy(i => i.MaturityDate)?.MaturityDate ?? default,
                TotalPurchaseAmount = Price.Create(instrumentsList.Sum(p => p.PurchaseAmount), currency),
                TotalRepaymentAmount = Price.Create(instrumentsList.Sum(p => p.RepaymentAmount), currency),
                ReferenceNumber = note.ReferenceNumber,
                Instruments = [.. instrumentsList.Select(i => new InstrumentDto
                {
                    Id = i.Id,
                    MaturityDate = i.MaturityDate,
                    IssueDate = i.IssueDate,
                    PurchaseAmount = Price.Create(i.PurchaseAmount, currency),
                    RepaymentAmount = Price.Create(i.RepaymentAmount, currency),
                    Type = i.Type.ToString(),
                })],
            };
        }
    }
}
