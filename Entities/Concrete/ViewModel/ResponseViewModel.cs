namespace Entities.Concrete.ViewModel;

public class ResponseViewModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public TokenInfo TokenInfo { get; set; } = null!;
}