using System.ComponentModel.DataAnnotations;
namespace Dziennik_elektroniczny.Models;


public class Administrator
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

    [Required(ErrorMessage = "Login jest wymagany.")]
    [MaxLength(50, ErrorMessage = "Login nie może mieć więcej niż 50 znaków.")]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
    public string? Haslo { get; set; }
}



