namespace Shortify.Domain.Abstractions.Errors;

public record Error(string Code, ErrorType Type, string Description = "")
{
    public static readonly Error None = new(string.Empty, ErrorType.None);

    public static Error CreateError(ErrorType errorType, string code)
    {
        return new Error(code, errorType);
    }

    public static Error NotFound(string code = "Application.NotFound") =>
        new Error(code, ErrorType.NotFound);

    public static Error Conflict(string code = "Application.Conflict") =>
        new Error(code, ErrorType.Conflict);

    public static Error Validation(string code = "Application.Validation") =>
        new Error(code, ErrorType.Validation);

    public static Error Unauthorized(string code = "Application.Unauthorized") =>
        new Error(code, ErrorType.Unauthorized);

    public static Error Forbidden(string code = "Application.Forbidden") =>
        new Error(code, ErrorType.Forbidden);

    public static Error BadRequest(string code = "Application.BadRequest") =>
        new Error(code, ErrorType.BadRequest);

    public static Error InternalServerError(string code = "Application.InternalServerError") =>
        new Error(code, ErrorType.InternalServerError);

    public static Error ServiceUnavailable(string code = "Application.ServiceUnavailable") =>
        new Error(code, ErrorType.ServiceUnavailable);

    public static Error TooManyRequests(string code = "Application.TooManyRequests") =>
        new Error(code, ErrorType.TooManyRequests);

    public static Error UnprocessableEntity(string code = "Application.UnprocessableEntity") =>
        new Error(code, ErrorType.UnprocessableEntity);
}