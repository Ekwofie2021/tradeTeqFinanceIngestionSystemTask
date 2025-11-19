using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Queries.GetInstrument;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Tests.Queries
{
    public class GetInstrumentQueryHandlerTests
    {
        private readonly Mock<IInstrumentRepository> _mockRepository;
        private readonly GetInstrumentQueryHandler _handler;

        public GetInstrumentQueryHandlerTests()
        {
            _mockRepository = new Mock<IInstrumentRepository>();
            _handler = new GetInstrumentQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_InstrumentDto_When_Instrument_Exists()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var instrument = new Instrument
            {
                Id = instrumentId,
                NoteId = Guid.NewGuid(),
                Type = Type.RECEIVABLE,
                IssueDate = new DateTime(2025, 1, 1),
                MaturityDate = new DateTime(2025, 12, 31),
                Currency = Currency.USD,
                PurchaseAmount = 10000,
                RepaymentAmount = 11000
            };

            _mockRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(instrument);

            var query = new GetInstrumentQuery { InstrumentId = instrumentId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be("RECEIVABLE");
            result.IssueDate.Should().Be(new DateTime(2025, 1, 1));
            result.MaturityDate.Should().Be(new DateTime(2025, 12, 31));
            result.PurchaseAmount.Should().NotBeNull();
            result.PurchaseAmount!.Amount.Should().Be(10000);
            result.PurchaseAmount.Currency.Should().Be("USD");
            result.RepaymentAmount.Should().NotBeNull();
            result.RepaymentAmount!.Amount.Should().Be(11000);
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Instrument_Does_Not_Exist()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            _mockRepository
                .Setup(r => r.GetInstrumentByIdAsync(instrumentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Instrument?)null);

            var query = new GetInstrumentQuery { InstrumentId = instrumentId };

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"InstrumentId {instrumentId} not found");
        }
    }
}
