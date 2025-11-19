using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Commands.CreateNote;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Xunit;

namespace TradeFinanceIngestionSystem.Tests.Commands
{
    public class UpdateNoteStatusCommandHandlerTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly UpdateNoteStatusCommandHandler _handler;

        public UpdateNoteStatusCommandHandlerTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _handler = new UpdateNoteStatusCommandHandler(_mockNoteRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Note_Status_From_Draft_To_Published()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 12345,
                IssueDate = DateTime.UtcNow,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockNoteRepository
                .Setup(r => r.UpdateNoteStatus(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateNoteStatusCommand
            {
                NoteId = noteId,
                Status = "PUBLISHED"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(noteId);
            _mockNoteRepository.Verify(
                r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()), 
                Times.Once
            );
            _mockNoteRepository.Verify(
                r => r.UpdateNoteStatus(It.Is<Note>(n => 
                    n.Id == noteId && 
                    n.Status == Status.PUBLISHED
                ), It.IsAny<CancellationToken>()), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Update_Note_Status_From_Published_To_Draft()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 67890,
                IssueDate = DateTime.UtcNow,
                Status = Status.PUBLISHED
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockNoteRepository
                .Setup(r => r.UpdateNoteStatus(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateNoteStatusCommand
            {
                NoteId = noteId,
                Status = "DRAFT"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(noteId);
            _mockNoteRepository.Verify(
                r => r.UpdateNoteStatus(It.Is<Note>(n => 
                    n.Id == noteId && 
                    n.Status == Status.DRAFT
                ), It.IsAny<CancellationToken>()), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Parse_Status_Case_Insensitively()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 11111,
                IssueDate = DateTime.UtcNow,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockNoteRepository
                .Setup(r => r.UpdateNoteStatus(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateNoteStatusCommand
            {
                NoteId = noteId,
                Status = "published" // lowercase
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(noteId);
            _mockNoteRepository.Verify(
                r => r.UpdateNoteStatus(It.Is<Note>(n => 
                    n.Status == Status.PUBLISHED
                ), It.IsAny<CancellationToken>()), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Note_Does_Not_Exist()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Note?)null);

            var command = new UpdateNoteStatusCommand
            {
                NoteId = noteId,
                Status = "PUBLISHED"
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"InstrumentId {noteId} not found");

            _mockNoteRepository.Verify(
                r => r.UpdateNoteStatus(It.IsAny<Note>(), It.IsAny<CancellationToken>()), 
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_ArgumentException_When_Status_Is_Invalid()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 22222,
                IssueDate = DateTime.UtcNow,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            var command = new UpdateNoteStatusCommand
            {
                NoteId = noteId,
                Status = "INVALID_STATUS"
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid status: Status");

            _mockNoteRepository.Verify(
                r => r.UpdateNoteStatus(It.IsAny<Note>(), It.IsAny<CancellationToken>()), 
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Correct_NoteId_After_Update()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note
            {
                Id = noteId,
                ReferenceNumber = 33333,
                IssueDate = DateTime.UtcNow,
                Status = Status.DRAFT
            };

            _mockNoteRepository
                .Setup(r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            _mockNoteRepository
                .Setup(r => r.UpdateNoteStatus(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateNoteStatusCommand
            {
                NoteId = noteId,
                Status = "PUBLISHED"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(noteId);
            result.Should().NotBe(Guid.Empty);
        }
    }
}
