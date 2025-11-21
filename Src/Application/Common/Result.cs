namespace Application.Common;

public sealed class Result
{
    public bool Success { get; init; }
    public string? Error { get; init; }

    public static Result Ok() => new() { Success = true };
    public static Result Fail(string error) => new() { Success = false, Error = error };
}

public sealed class Result<T>
{
    public bool Success { get; init; }
    public string? Error { get; init; }
    public T? Value { get; init; }

    public static Result<T> Ok(T value) => new() { Success = true, Value = value };
    public static Result<T> Fail(string error) => new() { Success = false, Error = error };
}