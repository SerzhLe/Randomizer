namespace Randomizer.Common;

public class Result
{
    protected Result() { }

    public bool IsSuccessful => !ApiErrorCode.HasValue && !ValidationErrors.Any();

    public string ErrorMessage { get; protected init; } = string.Empty;

    public int? ApiErrorCode { get; protected init; }

    public List<ValidationError> ValidationErrors { get; protected init; } = new();

    public static Result Success() => new Result();

    public static Result Error(string errorMessage, int apiErrorCode) 
        => new Result { ErrorMessage = errorMessage, ApiErrorCode = apiErrorCode };

    public static Result ValidationError(List<ValidationError> validationErrors) 
        => new Result { ValidationErrors = validationErrors };
}

public class Result<T> : Result
{
    public T? Payload { get; protected init; }

    public static Result<T> Success<T>(T payload) => new Result<T> { Payload = payload };

    public new static Result<T> Error(string errorMessage, int apiErrorCode) 
        => new Result<T> { ErrorMessage = errorMessage, ApiErrorCode = apiErrorCode };

    public new static Result<T> ValidationError(List<ValidationError> validationErrors) 
        => new Result<T> { ValidationErrors = validationErrors };
}

public record ValidationResult
{
    public bool IsValid { get; init; }

    public List<ValidationError> ValidationErrors { get; init; } = new();
}

public record ValidationError
{
    public string ErrorMessage { get; init; } = string.Empty;

    public string PropertyName { get; init; } = string.Empty;

    public object? CustomState { get; init; }
}