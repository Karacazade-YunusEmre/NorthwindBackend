using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.ViewModel;

public class RegisterViewModel
{
    [Required]
    [DisplayName("Ad")]
    [StringLength(60)]
    public string FirstName { get; set; } = null!;


    [Required]
    [DisplayName("Soyad")]
    [StringLength(60)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email adresi zorunlu")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre zorunlu")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Girmiş olduğunuz parola birbiri ile eşleşmiyor.")]
    public string ConfirmPassword { get; set; } = null!;
}