using FluentValidation;

namespace SurveyBasket.Api.Contracts.Validations;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(poll => poll.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(poll => poll.Description)
            .NotEmpty()
            .Length(3, 1000);

            
    }
}
