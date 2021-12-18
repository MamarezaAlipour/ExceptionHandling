namespace ExceptionHandling;

public class Result<T> : IResult<T>
{
    public T Data { get; set; }

    public int Code { get; set; }
    public string ErrorMessage { get; set; }


    public Result(T data, string errorMessage = null)
    {
        Data = data;
        ErrorMessage = errorMessage;
    }

    public Result(T data, int code, string errorMessage = null) : this(data, errorMessage)
        => Code = code;
}
