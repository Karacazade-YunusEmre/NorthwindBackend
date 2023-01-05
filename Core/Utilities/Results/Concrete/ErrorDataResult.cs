namespace Core.Utilities.Results.Concrete;

public class ErrorDataResult<T> : DataResult<T>
{
    public ErrorDataResult(string message, T data) : base(false, message, data)
    {
    }

    public ErrorDataResult(T data) : base(data, false)
    {
    }
}