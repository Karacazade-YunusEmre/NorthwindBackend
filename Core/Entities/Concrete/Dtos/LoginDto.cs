using System.ComponentModel.DataAnnotations;
using Core.Entities.Abstract;

namespace Core.Entities.Concrete.Dtos;

public class LoginDto : IDto
{
    [Required(ErrorMessage = "Email adresi zorunlu")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre zorunlu")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}