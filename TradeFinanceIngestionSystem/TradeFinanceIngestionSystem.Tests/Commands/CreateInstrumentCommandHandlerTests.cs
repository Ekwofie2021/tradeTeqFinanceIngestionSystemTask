using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Commands.CreateInstrument;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;

using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Tests.Commands
{
    public class CreateInstrumentCommandHandlerTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly Mock<IInstrumentRepository> _mockInstrumentRepository;
        private readonly CreateInstrumentCommandHandler _handler;

        public CreateInstrumentCommandHandlerTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _handler = new CreateInstrumentCommandHandler(
                _mockNoteRepository.Object,
                _mockInstrumentRepository.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Create_Instrument_Successfully()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = Currency.USD,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockInstrumentRepository
                .Setup(r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "RECEIVABLE",
                Currency = "USD",
                IssueDate = new DateTime(2025, 1, 1),
                MaturityDate = new DateTime(2025, 12, 31),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            
            _mockNoteRepository.Verify(
                r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()),
                Times.Once
            );
            
            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.Is<Instrument>(i =>
                    i.NoteId == noteId &&
                    i.Type == Type.RECEIVABLE &&
                    i.Currency == Currency.USD &&
                    i.IssueDate == new DateTime(2025, 1, 1) &&
                    i.MaturityDate == new DateTime(2025, 12, 31) &&
                    i.PurchaseAmount == 10000m &&
                    i.RepaymentAmount == 11000m
                ), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_ArgumentException_When_Note_Does_Not_Exist()
        {
            // Arrange
            var noteId = Guid.NewGuid();

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Note?)null);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "RECEIVABLE",
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"NoteId: {noteId} does not exist");

            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_ArgumentException_When_Type_Is_Invalid()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = Currency.USD,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "INVALID_TYPE",
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid type: Type");

            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_ArgumentException_When_Currency_Is_Invalid()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = Currency.USD,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "RECEIVABLE",
                Currency = "INVALID_CURRENCY",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid currency: Currency");

            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_ArgumentException_When_Currency_Does_Not_Match_Note_Currency()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = Currency.USD,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "RECEIVABLE",
                Currency = "EUR",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Instrument: Currency currency does not match Note currency");

            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Theory]
        [InlineData("RECEIVABLE", Type.RECEIVABLE)]
        [InlineData("GUARANTEE", Type.GUARANTEE)]
        [InlineData("LETTER_OF_CREDIT", Type.LETTER_OF_CREDIT)]
        [InlineData("receivable", Type.RECEIVABLE)]
        [InlineData("guarantee", Type.GUARANTEE)]
        public async Task Handle_Should_Parse_Type_Enum_Correctly(string typeString, Type expectedType)
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = Currency.USD,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockInstrumentRepository
                .Setup(r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = typeString,
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.Is<Instrument>(i => i.Type == expectedType), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Theory]
        [InlineData("USD", Currency.USD)]
        [InlineData("EUR", Currency.EUR)]
        [InlineData("GBP", Currency.GBP)]
        [InlineData("usd", Currency.USD)]
        [InlineData("eur", Currency.EUR)]
        public async Task Handle_Should_Parse_Currency_Enum_Correctly(string currencyString, Currency expectedCurrency)
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = expectedCurrency,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockInstrumentRepository
                .Setup(r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "RECEIVABLE",
                Currency = currencyString,
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000m,
                RepaymentAmount = 11000m
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.Is<Instrument>(i => i.Currency == expectedCurrency), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Use_Note_Currency_For_Instrument()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                Currency = Currency.EUR,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockInstrumentRepository
                .Setup(r => r.SaveInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateInstrumentCommand
            {
                NoteId = noteId,
                Type = "GUARANTEE",
                Currency = "EUR",
                IssueDate = new DateTime(2025, 3, 1),
                MaturityDate = new DateTime(2026, 3, 1),
                PurchaseAmount = 15000m,
                RepaymentAmount = 16500m
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _mockInstrumentRepository.Verify(
                r => r.SaveInstrumentAsync(It.Is<Instrument>(i => i.Currency == Currency.EUR), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
