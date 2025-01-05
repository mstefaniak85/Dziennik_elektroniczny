using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dziennik_elektroniczny.Models
{
    public class Uczen
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię ucznia jest wymagane.")]
        [MaxLength(50, ErrorMessage = "Imię ucznia nie może mieć więcej niż 50 znaków.")]
        public string? Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko ucznia jest wymagane.")]
        [MaxLength(50, ErrorMessage = "Nazwisko ucznia nie może mieć więcej niż 50 znaków.")]
        public string? Nazwisko { get; set; }

        [Required]
        [ForeignKey("Klasa")]
        public int KlasaId { get; set; }

        public Klasa? Klasa { get; set; }
    }
}
