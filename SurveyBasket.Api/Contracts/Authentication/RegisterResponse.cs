namespace SurveyBasket.Api.Contracts.Authentication;

public record RegisterResponse(
    string? Email,
    string FirstName,
    string LastName,
    string message
);
