using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dziennik_elektroniczny.Models
{
    public class Klasa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numer klasy jest wymagany.")]
        [Range(1, 8, ErrorMessage = "Numer klasy musi być od 1 do 8.")]
        public int Numer { get; set; } 
        [Required(ErrorMessage = "Litera klasy jest wymagana.")]
        [RegularExpression(@"^[A-E]$", ErrorMessage = "Litera klasy musi być jedną z liter: A, B, C, D, E.")]
        public string? Litera { get; set; } 

        public ICollection<Uczen> Uczniowie { get; set; } = new List<Uczen>();

        public string Nazwa => $"{Numer}{Litera}";
    }
}
