using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Core.Entities.Abstract;

namespace Entities.Concrete;

public class Product : IEntity
{
    [Required]
    [DisplayName("Id")]
    [Column(name: "ProductID")]
    public int Id { get; set; }

    [Required]
    [DisplayName("CategoryId")]
    [Column(name: "CategoryID")]
    public int CategoryId { get; set; }

    [Required]
    [DisplayName("ProductName")]
    [Column(name: "ProductName")]
    public string Name { get; set; } = null!;

    [DisplayName("QuantityPerUnit")]
    [Column(name: "QuantityPerUnit")]
    public string QuantityPerUnit { get; set; } = null!;

    [DisplayName("UnitPrice")]
    [Column(name: "UnitPrice")]
    public decimal UnitPrice { get; set; }

    [DisplayName("UnitsInStock")]
    [Column(name: "UnitsInStock")]
    public short UnitsInStock { get; set; }
}