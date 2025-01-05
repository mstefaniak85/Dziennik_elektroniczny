using System.ComponentModel.DataAnnotations;
namespace Dziennik_elektroniczny.Models;

public class Obecnosc
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informacja o obecności jest wymagana.")]
    public bool Obecny { get; set; }

    [Required(ErrorMessage = "Uczeń jest wymagany.")]
    public int UczenId { get; set; }

    public Uczen? Uczen { get; set; } 

    [Required(ErrorMessage = "Data zajęć jest wymagana.")]
    public DateTime DataZajec { get; set; } 
}