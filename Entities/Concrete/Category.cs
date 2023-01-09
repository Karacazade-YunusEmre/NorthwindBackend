using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Core.Entities.Abstract;

namespace Entities.Concrete;

public class Category : IEntity
{
    [Required]
    [DisplayName("Id")]
    [Column(name: "CategoryID")]
    public int Id { get; set; }

    [Required]
    [DisplayName("CategoryName")]
    [Column(name: "CategoryName")]
    public string Name { get; set; } = null!;
}