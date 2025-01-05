using System;
using System.ComponentModel.DataAnnotations;
namespace Dziennik_elektroniczny.Models;

public class Oceny
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ocena jest wymagana.")]
    [Range(1, 6, ErrorMessage = "Ocena musi być w zakresie od 1 do 6.")]
    public decimal Ocena { get; set; }

    [Required(ErrorMessage = "Data oceny jest wymagana.")]
    public DateTime DataOceny { get; set; }

    [Required(ErrorMessage = "Uczeń jest wymagany.")]
    public int UczenId { get; set; }

    public Uczen? Uczen { get; set; } 

    [Required(ErrorMessage = "Przedmiot jest wymagany.")]
    public int PrzedmiotId { get; set; }

    public Przedmiot? Przedmiot { get; set; } 
}
