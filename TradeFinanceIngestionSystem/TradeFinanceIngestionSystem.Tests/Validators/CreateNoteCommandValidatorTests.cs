using FluentAssertions;
using TradeFinanceIngestionSystem.Application.Commands.CreateNote;
using Xunit;

namespace TradeFinanceIngestionSystem.Tests.Validators
{
    public class CreateNoteCommandValidatorTests
    {
        private readonly CreateNoteCommandValidator _validator;

        public CreateNoteCommandValidatorTests()
        {
            _validator = new CreateNoteCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ReferenceNumber_Is_Zero()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = 0,
                IssueDate = DateTime.UtcNow,
                Currency = "USD"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ReferenceNumber");
        }

        [Fact]
        public void Should_Have_Error_When_ReferenceNumber_Is_Negative()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = -100,
                IssueDate = DateTime.UtcNow,
                Currency = "USD"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ReferenceNumber");
        }

        [Fact]
        public void Should_Have_Error_When_IssueDate_Is_Empty()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = 12345,
                IssueDate = default(DateTime),
                Currency = "USD"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "IssueDate");
        }

        [Fact]
        public void Should_Have_Error_When_Currency_Is_Empty()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = 12345,
                IssueDate = DateTime.UtcNow,
                Currency = string.Empty
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Currency");
        }

        [Fact]
        public void Should_Have_Error_When_Currency_Is_Null()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = 12345,
                IssueDate = DateTime.UtcNow,
                Currency = null!
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Currency");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = 12345,
                IssueDate = DateTime.UtcNow,
                Currency = "USD"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Should_Accept_Different_Currencies()
        {
            // Arrange
            var command = new CreateNoteCommand
            {
                ReferenceNumber = 99999,
                IssueDate = DateTime.UtcNow,
                Currency = "EUR"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
