namespace Randomizer.Common;
public class Result<T>
{
    protected Result() { }

    protected Result(T payload)
    {
        PayLoad = payload;
    }

    protected Result(string serverErrorMessage, int apiErrorCode)
    {
        ErrorMessage = serverErrorMessage;
        ApiErrorCode = apiErrorCode;
    }

    protected Result(List<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    public bool IsSuccessful => !ApiErrorCode.HasValue && !ValidationErrors.Any();

    public string? ErrorMessage { get; protected init; }

    public int? ApiErrorCode { get; protected init; }

    public List<ValidationError> ValidationErrors { get; protected init; } = new();

    public T? PayLoad { get; protected init; }

    public static Result<T> Success<T>(T payload) => new(payload);

    public static Result<T> Success() => new() { PayLoad = default };

    public static Result<T> Error(string errorMessage, int apiErrorCode) => new(errorMessage, apiErrorCode);

    public static Result<T> ValidationError(List<ValidationError> validationErrors) => new(validationErrors);
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