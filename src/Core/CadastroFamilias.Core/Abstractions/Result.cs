namespace CadastroFamilias.Core.Abstractions;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Sucesso não pode ter erro");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Falha precisa ter erro");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new(default!, false, error);
}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T value) => Success(value);
}

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Valor nulo não é permitido");
    public static readonly Error ValidationFailed = new("Error.Validation", "Falha na validação dos dados");
    public static readonly Error NotFound = new("Error.NotFound", "Recurso não encontrado");
    public static readonly Error InternalServer = new("Error.InternalServer", "Erro interno no servidor");
}