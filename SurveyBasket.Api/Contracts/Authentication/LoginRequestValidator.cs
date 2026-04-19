using FluentValidation;
namespace SurveyBasket.Api.Contracts.Poll;

public class LoginRequestValidator : AbstractValidator<PollRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Summary)
            .NotEmpty()
            .Length(3, 1500);

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt)
            .GreaterThan(x => x.StartsAt);




    }
}
