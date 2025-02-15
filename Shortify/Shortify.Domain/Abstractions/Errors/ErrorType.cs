namespace Shortify.Domain.Abstractions.Errors;

public enum ErrorType
{
    None,
    NotFound,
    Conflict,
    Validation,
    Unauthorized,
    Forbidden,
    BadRequest,
    InternalServerError,
    ServiceUnavailable,
    TooManyRequests,
    UnprocessableEntity
}
