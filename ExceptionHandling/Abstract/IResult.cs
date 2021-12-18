namespace ExceptionHandling;

public interface IResult
{
    int Code { get; set; }
    string ErrorMessage { get; set; }
}

public interface IResult<T> : IResult
{
    T Data { get; set; }
}
