using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using TradeFinanceIngestionSystem.Infrastructure.DbContexts;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Infrastructure.SeedData
{
    public static class DatabaseSeeder
    {
        public static void SeedData(NoteDataContext noteContext, InstrumentDataContext instrumentContext)
        {
            SeedNotes(noteContext);
            SeedInstruments(instrumentContext);
        }

        private static void SeedNotes(NoteDataContext context)
        {
            if (context.Notes.Any())
            {
                return; // Database already seeded
            }

            var notes = new List<Note>
            {
                new() {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ReferenceNumber = 10001,
                    Status = Status.DRAFT,
                    IssueDate = new DateTime(2025, 1, 15),
                    Currency = Currency.USD
                },
                new() {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ReferenceNumber = 10002,
                    Status = Status.PUBLISHED,
                    IssueDate = new DateTime(2025, 2, 1),
                    Currency = Currency.GBP
                },
                new() {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ReferenceNumber = 10003,
                    Status = Status.PUBLISHED,
                    IssueDate = new DateTime(2025, 3, 10),
                    Currency = Currency.EUR
                },
                new() {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ReferenceNumber = 10004,
                    Status = Status.DRAFT,
                    IssueDate = new DateTime(2025, 4, 5),
                    Currency = Currency.USD
                },
                new Note
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ReferenceNumber = 10005,
                    Status = Status.PUBLISHED,
                    IssueDate = new DateTime(2025, 5, 20),
                    Currency = Currency.GBP
                }
            };

            context.Notes.AddRange(notes);
            context.SaveChanges();
        }

        private static void SeedInstruments(InstrumentDataContext context)
        {
            if (context.Instruments.Any())
            {
                return; // Database already seeded
            }

            var instruments = new List<Instrument>
            {
                // Instruments for Note 1 (USD)
                new Instrument
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    NoteId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 15),
                    MaturityDate = new DateTime(2025, 7, 15),
                    Currency = Currency.USD,
                    PurchaseAmount = 50000,
                    RepaymentAmount = 52500
                },
                new Instrument
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    NoteId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 1, 15),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.USD,
                    PurchaseAmount = 25000,
                    RepaymentAmount = 26000
                },

                // Instruments for Note 2 (GBP)
                new Instrument
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    NoteId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Type = Type.LETTER_OF_CREDIT,
                    IssueDate = new DateTime(2025, 2, 1),
                    MaturityDate = new DateTime(2025, 8, 1),
                    Currency = Currency.GBP,
                    PurchaseAmount = 75000,
                    RepaymentAmount = 78750
                },
                new Instrument
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    NoteId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 2, 1),
                    MaturityDate = new DateTime(2025, 11, 1),
                    Currency = Currency.GBP,
                    PurchaseAmount = 30000,
                    RepaymentAmount = 31500
                },

                // Instruments for Note 3 (EUR)
                new Instrument
                {
                    Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    NoteId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 3, 10),
                    MaturityDate = new DateTime(2025, 9, 10),
                    Currency = Currency.EUR,
                    PurchaseAmount = 100000,
                    RepaymentAmount = 105000
                },

                // Instruments for Note 4 (USD)
                new Instrument
                {
                    Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    NoteId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 4, 5),
                    MaturityDate = new DateTime(2025, 10, 5),
                    Currency = Currency.USD,
                    PurchaseAmount = 40000,
                    RepaymentAmount = 42000
                },
                new() {
                    Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                    NoteId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Type = Type.LETTER_OF_CREDIT,
                    IssueDate = new DateTime(2025, 4, 5),
                    MaturityDate = new DateTime(2026, 1, 5),
                    Currency = Currency.USD,
                    PurchaseAmount = 60000,
                    RepaymentAmount = 63600
                },
                new Instrument
                {
                    Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                    NoteId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 4, 5),
                    MaturityDate = new DateTime(2025, 7, 5),
                    Currency = Currency.USD,
                    PurchaseAmount = 15000,
                    RepaymentAmount = 15750
                },

                // Instruments for Note 5 (GBP)
                new Instrument
                {
                    Id = Guid.Parse("30303030-3030-3030-3030-303030303030"),
                    NoteId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 5, 20),
                    MaturityDate = new DateTime(2025, 11, 20),
                    Currency = Currency.GBP,
                    PurchaseAmount = 85000,
                    RepaymentAmount = 89250
                },
                new Instrument
                {
                    Id = Guid.Parse("40404040-4040-4040-4040-404040404040"),
                    NoteId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Type = Type.LETTER_OF_CREDIT,
                    IssueDate = new DateTime(2025, 5, 20),
                    MaturityDate = new DateTime(2026, 5, 20),
                    Currency = Currency.GBP,
                    PurchaseAmount = 120000,
                    RepaymentAmount = 126000
                }
            };

            context.Instruments.AddRange(instruments);
            context.SaveChanges();
        }
    }
}
