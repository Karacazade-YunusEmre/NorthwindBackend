namespace Core.Utilities.Results.Concrete;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(string message, T data) : base(true, message, data)
    {
    }

    public SuccessDataResult(T data) : base(data, true)
    {
    }
}