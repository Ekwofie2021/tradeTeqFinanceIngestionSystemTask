using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradeFinanceIngestionSystem.Application.Commands.CreateNote;
using TradeFinanceIngestionSystem.Application.Commands.DeleteNote;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;
using TradeFinanceIngestionSystem.Application.Queries.GetNote;
using TradeFinanceIngestionSystem.Application.Queries.GetNotes;
using TradeFinanceIngestionSystem.DTOs.Requests;

namespace TradeFinanceIngestionSystem.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDetailDto>>> GetNotes(int pageNumber = 1, int pageSize = 10)
        {
            var notes = await _mediator.Send(new GetNotesQuery
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            });

            return Ok(notes);
        }

        [HttpGet("{id}", Name = nameof(GetNoteAsync))]
        public async Task<ActionResult<NoteDetailDto>> GetNoteAsync(Guid id)
        {
            try
            {
                var note = await _mediator.Send(new GetNoteWithInstrumentQuery
                {
                    NoteId = id
                });

                if (note is null)
                {
                    return NotFound();
                }

                return Ok(note);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var noteId = await _mediator.Send(new CreateNoteCommand
            {
                IssueDate = request.IssueDate,
                ReferenceNumber = request.ReferenceNumber,
                Currency = request.Currency,
            });

            var respond = await _mediator.Send(new GetNoteQuery { NoteId = noteId });

            return CreatedAtRoute(nameof(GetNoteAsync), new { id = noteId }, new { respond });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNoteStatus(Guid id, UpdateNoteStatusRequest request)
        {

            if (request is null)
            {
                return BadRequest();
            }

            await _mediator.Send(new UpdateNoteStatusCommand
            {
                NoteId = id,
                Status = request.Status
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteNoteCommand{ NoteId = id });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
