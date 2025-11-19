using FluentAssertions;
using TradeFinanceIngestionSystem.Application.Mapper;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Xunit;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Tests.Mappers
{
    public class NoteWithInstrumentMapperTests
    {
        [Fact]
        public void MapToDto_Should_Calculate_TotalPurchaseAmount_Correctly()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                Status = Status.DRAFT,
                IssueDate = new DateTime(2025, 1, 1),
                Currency = Currency.USD
            };

            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.USD,
                    PurchaseAmount = 10000,
                    RepaymentAmount = 10500
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.USD,
                    PurchaseAmount = 25000,
                    RepaymentAmount = 26250
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.LETTER_OF_CREDIT,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.USD,
                    PurchaseAmount = 15000,
                    RepaymentAmount = 15750
                }
            };

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert
            result.TotalPurchaseAmount.Should().NotBeNull();
            result.TotalPurchaseAmount!.Amount.Should().Be(50000); // 10000 + 25000 + 15000
            result.TotalPurchaseAmount.Currency.Should().Be("USD");
        }

        [Fact]
        public void MapToDto_Should_Calculate_TotalRepaymentAmount_Correctly()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                Status = Status.DRAFT,
                IssueDate = new DateTime(2025, 1, 1),
                Currency = Currency.GBP
            };

            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.GBP,
                    PurchaseAmount = 20000,
                    RepaymentAmount = 21000
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.GBP,
                    PurchaseAmount = 30000,
                    RepaymentAmount = 31500
                }
            };

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert
            result.TotalRepaymentAmount.Should().NotBeNull();
            result.TotalRepaymentAmount!.Amount.Should().Be(52500); // 21000 + 31500
            result.TotalRepaymentAmount.Currency.Should().Be("GBP");
        }

        [Fact]
        public void MapToDto_Should_Calculate_FinalMaturityDate_AsLatestDate()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                Status = Status.PUBLISHED,
                IssueDate = new DateTime(2025, 1, 1),
                Currency = Currency.EUR
            };

            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 6, 30), // Earlier
                    Currency = Currency.EUR,
                    PurchaseAmount = 10000,
                    RepaymentAmount = 10500
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 2, 1),
                    MaturityDate = new DateTime(2026, 12, 31), // Latest - should be final
                    Currency = Currency.EUR,
                    PurchaseAmount = 20000,
                    RepaymentAmount = 21000
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.LETTER_OF_CREDIT,
                    IssueDate = new DateTime(2025, 3, 1),
                    MaturityDate = new DateTime(2025, 9, 30), // Middle
                    Currency = Currency.EUR,
                    PurchaseAmount = 15000,
                    RepaymentAmount = 15750
                }
            };

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert
            result.FinalMaturityDate.Should().Be(new DateTime(2026, 12, 31));
        }

        [Fact]
        public void MapToDto_Should_Handle_EmptyInstruments_WithZeroTotals()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                Status = Status.DRAFT,
                IssueDate = new DateTime(2025, 1, 1),
                Currency = Currency.USD
            };

            var instruments = new List<Instrument>();

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert
            result.TotalPurchaseAmount.Should().NotBeNull();
            result.TotalPurchaseAmount!.Amount.Should().Be(0);
            result.TotalRepaymentAmount.Should().NotBeNull();
            result.TotalRepaymentAmount!.Amount.Should().Be(0);
            result.FinalMaturityDate.Should().Be(default(DateTime));
            result.Instruments.Should().BeEmpty();
        }

        [Fact]
        public void MapToDto_Should_Calculate_AllComputedFields_WithSingleInstrument()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 99999,
                Status = Status.PUBLISHED,
                IssueDate = new DateTime(2025, 5, 1),
                Currency = Currency.USD
            };

            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 5, 1),
                    MaturityDate = new DateTime(2025, 11, 1),
                    Currency = Currency.USD,
                    PurchaseAmount = 100000,
                    RepaymentAmount = 105000
                }
            };

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert - All computed fields should equal single instrument values
            result.TotalPurchaseAmount!.Amount.Should().Be(100000);
            result.TotalRepaymentAmount!.Amount.Should().Be(105000);
            result.FinalMaturityDate.Should().Be(new DateTime(2025, 11, 1));
            result.Instruments.Should().HaveCount(1);
        }

        [Fact]
        public void MapToDto_Should_UseNoteCurrency_ForTotalAmounts()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                Status = Status.DRAFT,
                IssueDate = new DateTime(2025, 1, 1),
                Currency = Currency.GBP // Note currency
            };

            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.GBP,
                    PurchaseAmount = 50000,
                    RepaymentAmount = 52500
                }
            };

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert - Should use note currency, not instrument currency
            result.TotalPurchaseAmount!.Currency.Should().Be("GBP");
            result.TotalRepaymentAmount!.Currency.Should().Be("GBP");
        }

        [Fact]
        public void MapToDto_Should_Calculate_WithDecimalPrecision()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                Status = Status.DRAFT,
                IssueDate = new DateTime(2025, 1, 1),
                Currency = Currency.EUR
            };

            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.EUR,
                    PurchaseAmount = 12345.67m,
                    RepaymentAmount = 13000.12m
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = noteId,
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.EUR,
                    PurchaseAmount = 23456.78m,
                    RepaymentAmount = 24500.34m
                }
            };

            // Act
            var result = NoteWithInstrumentMapper.MapToDto(note, instruments);

            // Assert - Should maintain decimal precision
            result.TotalPurchaseAmount!.Amount.Should().Be(35802.45m); // 12345.67 + 23456.78
            result.TotalRepaymentAmount!.Amount.Should().Be(37500.46m); // 13000.12 + 24500.34
        }
    }
}
