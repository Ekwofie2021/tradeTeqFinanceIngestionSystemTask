using MediatR;

namespace TradeFinanceIngestionSystem.Application.Commands.DeleteInstrument
{
    public class DeleteInstrumentCommand : IRequest<bool>
    {
        public Guid InstrumentId { get; set; }
    }
}
