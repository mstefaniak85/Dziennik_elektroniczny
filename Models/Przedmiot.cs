using System.ComponentModel.DataAnnotations;
namespace Dziennik_elektroniczny.Models;

public class Przedmiot
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa przedmiotu jest wymagana.")]
    [StringLength(50, ErrorMessage = "Nazwa przedmiotu może mieć maksymalnie 50 znaków.")]
    public string? Nazwa { get; set; }

    [Required(ErrorMessage = "Nauczyciel jest wymagany.")]
    public int NauczycielId { get; set; }

    public Nauczyciel? Nauczyciel { get; set; } 
}
