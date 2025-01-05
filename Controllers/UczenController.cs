using Dziennik_elektroniczny.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Uczen")]
public class UczenController : Controller
{
    private readonly DziennikContext _context;

    public UczenController(DziennikContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        ViewData["Message"] = "Panel ucznia";
        return View();
    }

    public async Task<IActionResult> Oceny()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var uczen = await _context.Uczniowie
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (uczen == null)
        {
            TempData["Error"] = "Nie znaleziono danych ucznia.";
            return RedirectToAction("Dashboard");
        }

        var oceny = await _context.Oceny
            .Where(o => o.UczenId == uczen.Id)
            .Include(o => o.Przedmiot)
            .ToListAsync();

        return View(oceny);
    }

    public async Task<IActionResult> Obecnosci()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var uczen = await _context.Uczniowie
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (uczen == null)
        {
            TempData["Error"] = "Nie znaleziono danych ucznia.";
            return RedirectToAction("Dashboard");
        }

        var obecnosci = await _context.Obecnosci
            .Where(o => o.UczenId == uczen.Id)
            .ToListAsync();

        return View(obecnosci);
    }
}
