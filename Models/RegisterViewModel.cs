using System.ComponentModel.DataAnnotations;

namespace Dziennik_elektroniczny.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Nazwa użytkownika musi mieć od 3 do 30 znaków.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Nazwa użytkownika może zawierać tylko litery i cyfry.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane.")]
        [Compare("Password", ErrorMessage = "Hasła muszą być takie same.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Rola użytkownika jest wymagana.")]
        [RegularExpression(@"^(Uczen|Nauczyciel|Administrator)$", ErrorMessage = "Nieprawidłowa rola użytkownika.")]
        public string? Role { get; set; }
    }
}
