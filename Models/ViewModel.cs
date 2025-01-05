using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dziennik_elektroniczny.Models.ViewModels
{
    public class DodajPrzedmiotViewModel
    {
        public int? PrzedmiotId { get; set; } 
        public IEnumerable<SelectListItem>? Przedmioty { get; set; } 
    }
}