using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Application.Queries.GetInstruments;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Type = TradeFinanceIngestionSystem.Domain.Enums.Type;

namespace TradeFinanceIngestionSystem.Tests.Queries
{
    public class GetInstrumentsQueryHandlerTests
    {
        private readonly Mock<IInstrumentRepository> _mockRepository;
        private readonly GetInstrumentsQueryHandler _handler;

        public GetInstrumentsQueryHandlerTests()
        {
            _mockRepository = new Mock<IInstrumentRepository>();
            _handler = new GetInstrumentsQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_PagedResult_With_Instruments()
        {
            // Arrange
            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = Guid.NewGuid(),
                    Type = Type.RECEIVABLE,
                    IssueDate = new DateTime(2025, 1, 1),
                    MaturityDate = new DateTime(2025, 12, 31),
                    Currency = Currency.USD,
                    PurchaseAmount = 10000,
                    RepaymentAmount = 11000
                },
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = Guid.NewGuid(),
                    Type = Type.GUARANTEE,
                    IssueDate = new DateTime(2025, 2, 1),
                    MaturityDate = new DateTime(2025, 11, 30),
                    Currency = Currency.EUR,
                    PurchaseAmount = 20000,
                    RepaymentAmount = 22000
                }
            };

            _mockRepository
                .Setup(r => r.GetAllInstrumentsAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(instruments);

            _mockRepository
                .Setup(r => r.GetInstrumentsCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);

            var query = new GetInstrumentsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(2);
            result.TotalPages.Should().Be(1);
            result.HasPreviousPage.Should().BeFalse();
            result.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_PagedResult_When_No_Instruments()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetAllInstrumentsAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Instrument>());

            _mockRepository
                .Setup(r => r.GetInstrumentsCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var query = new GetInstrumentsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);
        }

        [Fact]
        public async Task Handle_Should_Calculate_Pagination_Metadata_Correctly()
        {
            // Arrange
            var instruments = new List<Instrument>
            {
                new Instrument
                {
                    Id = Guid.NewGuid(),
                    NoteId = Guid.NewGuid(),
                    Type = Type.RECEIVABLE,
                    IssueDate = DateTime.UtcNow,
                    MaturityDate = DateTime.UtcNow.AddMonths(12),
                    Currency = Currency.USD,
                    PurchaseAmount = 10000,
                    RepaymentAmount = 11000
                }
            };

            _mockRepository
                .Setup(r => r.GetAllInstrumentsAsync(2, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(instruments);

            _mockRepository
                .Setup(r => r.GetInstrumentsCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(15); // Total of 15 items

            var query = new GetInstrumentsQuery { PageNumber = 2, PageSize = 5 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.PageNumber.Should().Be(2);
            result.TotalPages.Should().Be(3); // 15 / 5 = 3 pages
            result.HasPreviousPage.Should().BeTrue();
            result.HasNextPage.Should().BeTrue();
        }
    }
}
