using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Commands.DeleteInstrument;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Xunit;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Tests.Commands
{
    public class DeleteInstrumentCommandHandlerTests
    {
        private readonly Mock<IInstrumentRepository> _mockInstrumentRepository;
        private readonly DeleteInstrumentCommandHandler _handler;

        public DeleteInstrumentCommandHandlerTests()
        {
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _handler = new DeleteInstrumentCommandHandler(_mockInstrumentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_Instrument_Successfully()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var instrument = new Instrument
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
                .ReturnsAsync(instrument);

            _mockInstrumentRepository
                .Setup(r => r.DeleteInstrumentAsync(instrument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new DeleteInstrumentCommand { InstrumentId = instrumentId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockInstrumentRepository.Verify(
                r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()), 
                Times.Once
            );
            _mockInstrumentRepository.Verify(
                r => r.DeleteInstrumentAsync(instrument, It.IsAny<CancellationToken>()), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Instrument_Does_Not_Exist()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            _mockInstrumentRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Instrument?)null);

            var command = new DeleteInstrumentCommand { InstrumentId = instrumentId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"The instrument with ID: {instrumentId} cannot be found");

            _mockInstrumentRepository.Verify(
                r => r.DeleteInstrumentAsync(It.IsAny<Instrument>(), It.IsAny<CancellationToken>()), 
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Repository_Fails_To_Delete()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var instrument = new Instrument
            {
                Id = instrumentId,
                NoteId = Guid.NewGuid(),
                Type = Type.GUARANTEE,
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(6),
                PurchaseAmount = 5000,
                RepaymentAmount = 5500,
                Currency = Currency.EUR
            };

            _mockInstrumentRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(instrument);

            _mockInstrumentRepository
                .Setup(r => r.DeleteInstrumentAsync(instrument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new DeleteInstrumentCommand { InstrumentId = instrumentId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
