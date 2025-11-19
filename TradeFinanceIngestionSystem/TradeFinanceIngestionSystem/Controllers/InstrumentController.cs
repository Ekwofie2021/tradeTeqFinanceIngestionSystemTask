using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradeFinanceIngestionSystem.Application.Commands.CreateInstrument;
using TradeFinanceIngestionSystem.Application.Commands.DeleteInstrument;
using TradeFinanceIngestionSystem.Application.Commands.Update;
using TradeFinanceIngestionSystem.Application.Queries.DTOs;
using TradeFinanceIngestionSystem.Application.Queries.GetInstrument;
using TradeFinanceIngestionSystem.Application.Queries.GetInstruments;
using TradeFinanceIngestionSystem.DTOs.Requests;

namespace TradeFinanceIngestionSystem.Controllers
{
    [Route("api/instrument")]
    [ApiController]
    public class InstrumentController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<InstrumentDto>>> GetInstruments(int pageNumber = 1, int pageSize = 10)
        {
            var instruments = await _mediator.Send(new GetInstrumentsQuery
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            });

            return Ok(instruments);
        }

        [HttpGet("{id}", Name = nameof(GetInstrumentAsync))]
        public async Task<ActionResult<InstrumentDto>> GetInstrumentAsync(Guid id)
        {
            try
            {
                var instrument = await _mediator.Send(new GetInstrumentQuery
                {
                    InstrumentId = id
                });

                if (instrument is null)
                {
                    return NotFound();
                }

                return Ok(instrument);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateInstrument(CreateInstrumentRequest request)
        {
            try
            {
                var instrumentId = await _mediator.Send(new CreateInstrumentCommand
                {
                    NoteId = request.NoteId,
                    Type = request.Type,
                    Currency = request.Currency,
                    IssueDate = request.IssueDate,
                    MaturityDate = request.MaturityDate,
                    PurchaseAmount = request.PurchaseAmount,
                    RepaymentAmount = request.RepaymentAmount
                });

                var respond = await _mediator.Send(new GetInstrumentQuery
                {
                    InstrumentId = instrumentId
                });

                return CreatedAtRoute(nameof(GetInstrumentAsync), new { id = instrumentId }, respond);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInstrument(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteInstrumentCommand { InstrumentId = id });
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInstrument(Guid id, UpdateInstrumentRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var instrument = await _mediator.Send(new UpdateInstrumentCommand
            {
                InstrumentId = id,
                PurchaseAmount = request.PurchaseAmount,
                RepaymentAmount = request.RepaymentAmount,
                Type = request.Type,
                IssueDate = request.IssueDate,
                MaturityDate = request.MaturityDate,
            });

            if (instrument is null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
