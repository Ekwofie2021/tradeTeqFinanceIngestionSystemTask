using FluentAssertions;
using TradeFinanceIngestionSystem.Application.Commands.CreateInstrument;
using TradeFinanceIngestionSystem.Domain.Enums;
using Xunit;

namespace TradeFinanceIngestionSystem.Tests.Validators
{
    public class CreateInstrumentCommandValidatorTests
    {
        private readonly CreateInstrumentCommandValidator _validator;

        public CreateInstrumentCommandValidatorTests()
        {
            _validator = new CreateInstrumentCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_NoteId_Is_Empty()
        {
            // Arrange
            var command = new CreateInstrumentCommand
            {
                NoteId = Guid.Empty,
                Type = "RECEIVABLE",
                PurchaseAmount = 1000,
                RepaymentAmount = 1100,
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12)
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "NoteId");
        }

        [Fact]
        public void Should_Have_Error_When_PurchaseAmount_Is_Zero_Or_Negative()
        {
            // Arrange
            var command = new CreateInstrumentCommand
            {
                NoteId = Guid.NewGuid(),
                Type = "RECEIVABLE",
                PurchaseAmount = -100,
                RepaymentAmount = 1100,
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12)
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PurchaseAmount");
        }

        [Fact]
        public void Should_Have_Error_When_RepaymentAmount_Is_Zero_Or_Negative()
        {
            // Arrange
            var command = new CreateInstrumentCommand
            {
                NoteId = Guid.NewGuid(),
                Type = "RECEIVABLE",
                PurchaseAmount = 1000,
                RepaymentAmount = 0,
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12)
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "RepaymentAmount");
        }

        [Fact]
        public void Should_Have_Error_When_MaturityDate_Is_Before_IssueDate()
        {
            // Arrange
            var command = new CreateInstrumentCommand
            {
                NoteId = Guid.NewGuid(),
                Type = "RECEIVABLE",
                PurchaseAmount = 1000,
                RepaymentAmount = 1100,
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(-1)
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "MaturityDate");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateInstrumentCommand
            {
                NoteId = Guid.NewGuid(),
                Type = "RECEIVABLE",
                PurchaseAmount = 1000,
                RepaymentAmount = 1100,
                Currency = "USD",
                IssueDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddMonths(12)
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
