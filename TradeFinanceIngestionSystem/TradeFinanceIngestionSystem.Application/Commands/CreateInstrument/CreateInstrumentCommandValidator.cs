using FluentValidation;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateInstrument
{
    public class CreateInstrumentCommandValidator : AbstractValidator<CreateInstrumentCommand>
    {
        public CreateInstrumentCommandValidator()
        {
            RuleFor(x => x.NoteId)
                .NotEmpty()
                .WithMessage("NoteId is required");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Type must be a valid instrument type");

            RuleFor(x => x.PurchaseAmount)
                .GreaterThan(0)
                .WithMessage("PurchaseAmount must be greater than 0");

            RuleFor(x => x.RepaymentAmount)
                .GreaterThan(0)
                .WithMessage("RepaymentAmount must be greater than 0");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency must be a valid currency");

            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("IssueDate is required");

            RuleFor(x => x.MaturityDate)
                .NotEmpty()
                .WithMessage("MaturityDate is required")
                .GreaterThan(x => x.IssueDate)
                .WithMessage("MaturityDate must be after IssueDate");
        }
    }
}
