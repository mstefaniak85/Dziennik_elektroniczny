using System;
using System.ComponentModel.DataAnnotations;
namespace Dziennik_elektroniczny.Models;

public class Zajecia
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Data zajęć jest wymagana.")]
    public DateTime DataZajec { get; set; }

    [Required(ErrorMessage = "Klasa jest wymagana.")]
    public int KlasaId { get; set; }

    public Klasa? Klasa { get; set; } 

    [Required(ErrorMessage = "Przedmiot jest wymagany.")]
    public int PrzedmiotId { get; set; }

    public Przedmiot? Przedmiot { get; set; } 
}
