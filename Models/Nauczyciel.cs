using System.ComponentModel.DataAnnotations;
namespace Dziennik_elektroniczny.Models;

public class Nauczyciel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Imię jest wymagane.")]
    [MaxLength(50, ErrorMessage = "Imię nie może mieć więcej niż 50 znaków.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Imię może zawierać tylko litery i spacje.")]
    public string? Imie { get; set; }

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [MaxLength(50, ErrorMessage = "Nazwisko nie może mieć więcej niż 50 znaków.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Nazwisko może zawierać tylko litery i spacje.")]
    public string? Nazwisko { get; set; }

    [Required(ErrorMessage = "Przedmiot jest wymagany.")]
    [MaxLength(10, ErrorMessage = "Przedmiot nie może mieć więcej niż 50 znaków.")]
    public string? Przedmiot{ get; set; }
}
