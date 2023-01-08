using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email adresi zorunlu")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre zorunlu")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}