using MediatR;
using TradeFinanceIngestionSystem.Application.Interfaces;

namespace TradeFinanceIngestionSystem.Application.Commands.DeleteInstrument
{
    public class DeleteInstrumentCommandHandler(IInstrumentRepository _instrumentRepository) : IRequestHandler<DeleteInstrumentCommand, bool>
    {
        public async Task<bool> Handle(DeleteInstrumentCommand request, CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository.GetInstrumentByIdAsync(request.InstrumentId, cancellationToken)
                ?? throw new KeyNotFoundException($"The instrument with ID: {request.InstrumentId} cannot be found");

            var success = await _instrumentRepository.DeleteInstrumentAsync(instrument, cancellationToken);

            return success;
        }
    }
}
