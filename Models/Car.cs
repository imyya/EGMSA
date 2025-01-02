using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    [Range(2010, int.MaxValue, ErrorMessage = "L'année doit être supérieure ou égale à 2010.")]
    public int Year { get; set; }


    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
}
