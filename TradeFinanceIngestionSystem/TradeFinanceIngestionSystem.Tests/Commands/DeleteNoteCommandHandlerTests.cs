using FluentAssertions;
using Moq;
using TradeFinanceIngestionSystem.Application.Commands.DeleteNote;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Domain.Entities;
using TradeFinanceIngestionSystem.Domain.Enums;
using Xunit;

namespace TradeFinanceIngestionSystem.Tests.Commands
{
    public class DeleteNoteCommandHandlerTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly Mock<IInstrumentRepository> _mockInstrumentRepository;
        private readonly DeleteNoteCommandCommandHandler _handler;

        public DeleteNoteCommandHandlerTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _handler = new DeleteNoteCommandCommandHandler(
                _mockNoteRepository.Object,
                _mockInstrumentRepository.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Delete_Note_And_Associated_Instruments_Successfully()
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

            _mockInstrumentRepository
                .Setup(r => r.DeleteInstrumentsAssociatedToNoteByIdsAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockNoteRepository
                .Setup(r => r.DeleteNoteAsync(note, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new DeleteNoteCommand { NoteId = noteId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockNoteRepository.Verify(
                r => r.GetNoteByIdAsync(noteId, It.IsAny<CancellationToken>()), 
                Times.Once
            );
            _mockInstrumentRepository.Verify(
                r => r.DeleteInstrumentsAssociatedToNoteByIdsAsync(noteId, It.IsAny<CancellationToken>()), 
                Times.Once
            );
            _mockNoteRepository.Verify(
                r => r.DeleteNoteAsync(note, It.IsAny<CancellationToken>()), 
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

            var command = new DeleteNoteCommand { NoteId = noteId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"The Note with ID: {noteId} cannot be found");

            _mockInstrumentRepository.Verify(
                r => r.DeleteInstrumentsAssociatedToNoteByIdsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), 
                Times.Never
            );
            _mockNoteRepository.Verify(
                r => r.DeleteNoteAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()), 
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Repository_Fails_To_Delete()
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

            _mockInstrumentRepository
                .Setup(r => r.DeleteInstrumentsAssociatedToNoteByIdsAsync(noteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockNoteRepository
                .Setup(r => r.DeleteNoteAsync(note, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new DeleteNoteCommand { NoteId = noteId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
