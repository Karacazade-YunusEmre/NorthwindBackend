using Core.Utilities.Results.Abstract;

namespace Core.Utilities.Results.Concrete;

public class DataResult<T> : Result, IDataResult<T>
{
    protected DataResult(bool success, string message, T data) : base(success, message)
    {
        Data = data;
    }

    protected DataResult(T data, bool success) : base(success: success)
    {
        Data = data;
    }

    public T Data { get; }
}