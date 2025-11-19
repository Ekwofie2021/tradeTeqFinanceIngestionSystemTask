using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Commands.Update;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Xunit;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Tests.Commands
{
    public class UpdateInstrumentCommandHandlerTests
    {
        private readonly Mock<IInstrumentRepository> _mockInstrumentRepository;
        private readonly UpdateInstrumentCommandHandler _handler;

        public UpdateInstrumentCommandHandlerTests()
        {
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _handler = new UpdateInstrumentCommandHandler(_mockInstrumentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Instrument_Successfully()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var existingInstrument = new Instrument
            {
                Id = instrumentId,
                NoteId = Guid.NewGuid(),
                Type = Type.RECEIVABLE,
                IssueDate = new DateTime(2025, 1, 1),
                MaturityDate = new DateTime(2025, 12, 31),
                PurchaseAmount = 10000,
                RepaymentAmount = 11000,
                Currency = Currency.USD
            };

            var updatedInstrument = new Instrument
            {
                Id = instrumentId,
                NoteId = existingInstrument.NoteId,
                Type = Type.GUARANTEE,
                IssueDate = new DateTime(2025, 2, 1),
                MaturityDate = new DateTime(2026, 2, 1),
                PurchaseAmount = 15000,
                RepaymentAmount = 16500,
                Currency = Currency.EUR
            };

            _mockInstrumentRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInstrument);

            _mockInstrumentRepository
                .Setup(r => r.UpdateInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedInstrument);

            var command = new UpdateInstrumentCommand
            {
                InstrumentId = instrumentId,
                Type = "GUARANTEE",
                IssueDate = new DateTime(2025, 2, 1),
                MaturityDate = new DateTime(2026, 2, 1),
                PurchaseAmount = 15000,
                RepaymentAmount = 16500
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(instrumentId);
            result.Type.Should().Be(Type.GUARANTEE);
            result.PurchaseAmount.Should().Be(15000);
            result.RepaymentAmount.Should().Be(16500);
            
            _mockInstrumentRepository.Verify(
                r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()), 
                Times.Once
            );
            _mockInstrumentRepository.Verify(
                r => r.UpdateInstrumentAsync(It.Is<Instrument>(i => 
                    i.Id == instrumentId &&
                    i.Type == Type.GUARANTEE &&
                    i.PurchaseAmount == 15000 &&
                    i.RepaymentAmount == 16500
                ), It.IsAny<CancellationToken>()), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Update_All_Instrument_Properties()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var existingInstrument = new Instrument
            {
                Id = instrumentId,
                NoteId = Guid.NewGuid(),
                Type = Type.RECEIVABLE,
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000,
                RepaymentAmount = 11000,
                Currency = Currency.USD
            };

            _mockInstrumentRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInstrument);

            _mockInstrumentRepository
                .Setup(r => r.UpdateInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Instrument i, CancellationToken _) => i);

            var newIssueDate = new DateTime(2025, 3, 15);
            var newMaturityDate = new DateTime(2026, 3, 15);

            var command = new UpdateInstrumentCommand
            {
                InstrumentId = instrumentId,
                Type = "LETTER_OF_CREDIT",
                IssueDate = newIssueDate,
                MaturityDate = newMaturityDate,
                PurchaseAmount = 20000,
                RepaymentAmount = 22000
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be(Type.LETTER_OF_CREDIT);
            result.IssueDate.Should().Be(newIssueDate);
            result.MaturityDate.Should().Be(newMaturityDate);
            result.PurchaseAmount.Should().Be(20000);
            result.RepaymentAmount.Should().Be(22000);
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Instrument_Does_Not_Exist()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            _mockInstrumentRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Instrument?)null);

            var command = new UpdateInstrumentCommand
            {
                InstrumentId = instrumentId,
                Type = "RECEIVABLE",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000,
                RepaymentAmount = 11000
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"InstrumentId: {instrumentId} to be updated is not found");

            _mockInstrumentRepository.Verify(
                r => r.UpdateInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()), 
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Parse_Type_Enum_Correctly()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var existingInstrument = new Instrument
            {
                Id = instrumentId,
                NoteId = Guid.NewGuid(),
                Type = Type.RECEIVABLE,
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000,
                RepaymentAmount = 11000,
                Currency = Currency.USD
            };

            _mockInstrumentRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInstrument);

            _mockInstrumentRepository
                .Setup(r => r.UpdateInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Instrument i, CancellationToken _) => i);

            var command = new UpdateInstrumentCommand
            {
                InstrumentId = instrumentId,
                Type = "RECEIVABLE",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12),
                PurchaseAmount = 10000,
                RepaymentAmount = 11000
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Type.Should().Be(Type.RECEIVABLE);
        }
    }
}
