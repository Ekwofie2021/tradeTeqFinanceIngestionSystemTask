using FluentValidation;

namespace TradeFinanceIngestionSystem.Application.Commands.CreateNote
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.ReferenceNumber)
                .GreaterThan(0)
                .WithMessage("ReferenceNumber must be greater than 0");
            
            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("IssueDate is required");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency is required");
        }
    }
}
