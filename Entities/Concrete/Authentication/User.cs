using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete.Authentication;

public class User : IdentityUser, IEntity
{
    [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
    [DisplayName("Ad")]
    [StringLength(30)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]
    [DisplayName("Soyad")]
    [StringLength(30)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
    [DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Lütfen Email alanını belirtilen kriterlere uygun olarak doldurun")]
    [StringLength(30)]
    public override string Email { get; set; } = null!;

    [DisplayName("Adres")]
    [StringLength(60)]
    public string? Address { get; set; }
}