using System.Diagnostics.CodeAnalysis;
using WebApi.Common.Core.Result.Enums;

namespace WebApi.Common.Core.Result.Models;

public class Result<T>
{
    public static implicit operator Result<T>(T source) => Result<T>.Success(source);

    private Result() { }

    public T? Data { get; private set; }
    public IEnumerable<string>? Errors { get; private set; } = Enumerable.Empty<string>();
    public IEnumerable<FieldErrors>? FieldErrors { get; private set; } = Enumerable.Empty<FieldErrors>();
    public ResultStatus Status { get; private set; }
    public int? HttpStatusCode { get; private set; }

    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccess => Status == ResultStatus.Success;

    public static Result<T> Success(T? data = default)
    {
        return new Result<T>
        {
            Data = data,
            Status = ResultStatus.Success
        };
    }

    public static Result<T> Error(params ReadOnlySpan<string> errors)
    {
        if (errors.IsEmpty)
        {
            throw new ArgumentException("Errors cannot be empty.");
        }

        return new Result<T>
        {
            Errors = [.. errors],
            Status = ResultStatus.Error
        };
    }

    public static Result<T> ValidationError(IEnumerable<string>? errors = null, IEnumerable<FieldErrors>? fieldErrors = null)
    {
        if (errors?.Any() == false && fieldErrors?.Any() == false)
        {
            throw new ArgumentException("At least one error must be provided.");
        }

        return new Result<T>
        {
            Errors = errors ?? Enumerable.Empty<string>(),
            FieldErrors = fieldErrors ?? Enumerable.Empty<FieldErrors>(),
            Status = ResultStatus.ValidationError
        };
    }

    public static Result<T> NotFound(IEnumerable<string>? errors = null)
    {
        return new Result<T>
        {
            Errors = errors ?? Enumerable.Empty<string>(),
            Status = ResultStatus.NotFound
        };
    }

    public Result<T> WithHttpStatusCode(int? httpStatusCode)
    {
        HttpStatusCode = httpStatusCode;
        return this;
    }

    public Result<TOut> AsResultOfType<TOut>()
    {
        return new Result<TOut>
        {
            Errors = Errors,
            FieldErrors = FieldErrors,
            Status = Status,
            HttpStatusCode = HttpStatusCode
        };
    }
}
