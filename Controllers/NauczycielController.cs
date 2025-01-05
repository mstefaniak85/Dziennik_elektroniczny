using Dziennik_elektroniczny.Models;
using Dziennik_elektroniczny.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Nauczyciel")]
public class NauczycieleController : Controller
{
    private readonly DziennikContext _context;

    public NauczycieleController(DziennikContext context)
    {
        _context = context;
    }


    public IActionResult Dashboard()
    {
        ViewData["Message"] = "Panel nauczyciela";
        return View();
    }

    public async Task<IActionResult> Uczniowie()
    {
        var uczniowie = await _context.Uczniowie.Include(u => u.Klasa).ToListAsync();
        return View(uczniowie);
    }

    public IActionResult DodajUcznia()
    {
        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajUcznia(Uczen uczen)
    {
        if (ModelState.IsValid)
        {
            _context.Uczniowie.Add(uczen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Uczniowie));
        }

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa", uczen.KlasaId);
        return View(uczen);
    }

    public async Task<IActionResult> EdytujUcznia(int? id)
    {
        if (id == null) return NotFound();

        var uczen = await _context.Uczniowie.FindAsync(id);
        if (uczen == null) return NotFound();

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa", uczen.KlasaId);
        return View(uczen);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujUcznia(int id, Uczen uczen)
    {
        if (id != uczen.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(uczen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Uczniowie));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Uczniowie.Any(e => e.Id == uczen.Id)) return NotFound();
                throw;
            }
        }

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa", uczen.KlasaId);
        return View(uczen);
    }

    public async Task<IActionResult> Przedmioty()
    {
        var przedmioty = await _context.Przedmioty.Include(p => p.Nauczyciel).ToListAsync();
        return View(przedmioty);
    }


    public IActionResult DodajPrzedmiot()
    {
        if (!_context.Nauczyciele.Any())
        {
            TempData["Error"] = "Najpierw dodaj nauczycieli, zanim dodasz przedmiot.";
            return RedirectToAction(nameof(Przedmioty));
        }

        ViewData["Nauczyciele"] = new SelectList(_context.Nauczyciele.Select(n => new
        {
            Id = n.Id,
            ImieNazwisko = $"{n.Imie} {n.Nazwisko}"
        }), "Id", "ImieNazwisko");  

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajPrzedmiot(Przedmiot przedmiot)
    {
        if (ModelState.IsValid)
        {
            _context.Przedmioty.Add(przedmiot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Przedmioty));
        }

        ViewData["Nauczyciele"] = new SelectList(_context.Nauczyciele.Select(n => new
        {
            Id = n.Id,
            ImieNazwisko = $"{n.Imie} {n.Nazwisko}" 
        }), "Id", "ImieNazwisko", przedmiot.NauczycielId);

        return View(przedmiot);
    }

    public async Task<IActionResult> EdytujPrzedmiot(int? id)
    {
        if (id == null) return NotFound();

        var przedmiot = await _context.Przedmioty.Include(p => p.Nauczyciel).FirstOrDefaultAsync(p => p.Id == id);
        if (przedmiot == null) return NotFound();

        
        ViewData["Nauczyciele"] = new SelectList(_context.Nauczyciele.Select(n => new
        {
            Id = n.Id,
            ImieNazwisko = $"{n.Imie} {n.Nazwisko}" 
        }), "Id", "ImieNazwisko", przedmiot.NauczycielId);

        return View(przedmiot);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujPrzedmiot(int id, Przedmiot przedmiot)
    {
        if (id != przedmiot.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(przedmiot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Przedmioty));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Przedmioty.Any(p => p.Id == przedmiot.Id)) return NotFound();
                throw;
            }
        }

       
        ViewData["Nauczyciele"] = new SelectList(_context.Nauczyciele.Select(n => new
        {
            Id = n.Id,
            ImieNazwisko = $"{n.Imie} {n.Nazwisko}" 
        }), "Id", "ImieNazwisko", przedmiot.NauczycielId);

        return View(przedmiot);
    }

   
    public async Task<IActionResult> Zajecia()
    {
        var zajecia = await _context.Zajecia
            .Include(z => z.Klasa)
            .Include(z => z.Przedmiot)
            .ToListAsync();

        return View(zajecia);
    }

    public IActionResult DodajZajecia()
    {
        if (!_context.Klasy.Any() || !_context.Przedmioty.Any())
        {
            TempData["Error"] = "Najpierw dodaj klasy i przedmioty, zanim dodasz zajęcia.";
            return RedirectToAction(nameof(Zajecia));
        }

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa");
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajZajecia(Zajecia zajecia)
    {
        if (ModelState.IsValid)
        {
            _context.Zajecia.Add(zajecia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Zajecia));
        }

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa", zajecia.KlasaId);
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", zajecia.PrzedmiotId);
        return View(zajecia);
    }

    public async Task<IActionResult> EdytujZajecia(int? id)
    {
        if (id == null) return NotFound();

        var zajecia = await _context.Zajecia.FindAsync(id);
        if (zajecia == null) return NotFound();

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa", zajecia.KlasaId);
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", zajecia.PrzedmiotId);
        return View(zajecia);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujZajecia(int id, Zajecia zajecia)
    {
        if (id != zajecia.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(zajecia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Zajecia));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Zajecia.Any(z => z.Id == zajecia.Id)) return NotFound();
                throw;
            }
        }

        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa", zajecia.KlasaId);
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", zajecia.PrzedmiotId);
        return View(zajecia);
    }

    public async Task<IActionResult> Oceny()
    {
        var oceny = await _context.Oceny
            .Include(o => o.Uczen)
            .Include(o => o.Przedmiot)
            .ToListAsync();
        return View(oceny);
    }

    public IActionResult DodajOcene()
    {
        
        ViewBag.Uczniowie = new SelectList(_context.Uczniowie.Select(u => new
        {
            Id = u.Id,
            ImieNazwisko = $"{u.Imie} {u.Nazwisko}" 
        }), "Id", "ImieNazwisko");

        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajOcene(Oceny nowaOcena)
    {
        if (ModelState.IsValid)
        {
            _context.Oceny.Add(nowaOcena);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Oceny));
        }

     
        ViewBag.Uczniowie = new SelectList(_context.Uczniowie.Select(u => new
        {
            Id = u.Id,
            ImieNazwisko = $"{u.Imie} {u.Nazwisko}" 
        }), "Id", "ImieNazwisko", nowaOcena.UczenId);

        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", nowaOcena.PrzedmiotId);
        return View(nowaOcena);
    }

    public async Task<IActionResult> EdytujOcene(int? id)
    {
        if (id == null) return NotFound();

        var ocena = await _context.Oceny.FindAsync(id);
        if (ocena == null) return NotFound();

  
        ViewBag.Uczniowie = new SelectList(_context.Uczniowie.Select(u => new
        {
            Id = u.Id,
            ImieNazwisko = $"{u.Imie} {u.Nazwisko}" 
        }), "Id", "ImieNazwisko", ocena.UczenId);

        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", ocena.PrzedmiotId);
        return View(ocena);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujOcene(int id, Oceny ocenaDoAktualizacji)
    {
        if (id != ocenaDoAktualizacji.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ocenaDoAktualizacji);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Oceny));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Oceny.Any(o => o.Id == ocenaDoAktualizacji.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }


        ViewBag.Uczniowie = new SelectList(_context.Uczniowie.Select(u => new
        {
            Id = u.Id,
            ImieNazwisko = $"{u.Imie} {u.Nazwisko}"
        }), "Id", "ImieNazwisko", ocenaDoAktualizacji.UczenId);

        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", ocenaDoAktualizacji.PrzedmiotId);
        return View(ocenaDoAktualizacji);
    }

 
    public async Task<IActionResult> Obecnosci()
    {
        var obecnosci = await _context.Obecnosci
            .Include(o => o.Uczen) 
            .ToListAsync();
        return View(obecnosci);
    }

    public IActionResult DodajObecnosc()
    {
 
        var uczniowie = _context.Uczniowie
            .Select(u => new
            {
                u.Id,
                PelneImieNazwisko = u.Imie + " " + u.Nazwisko
            })
            .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "PelneImieNazwisko");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajObecnosc(Obecnosc nowaObecnosc)
    {
        if (ModelState.IsValid)
        {
            _context.Obecnosci.Add(nowaObecnosc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Obecnosci));
        }

        var uczniowie = _context.Uczniowie
            .Select(u => new
            {
                u.Id,
                PelneImieNazwisko = u.Imie + " " + u.Nazwisko
            })
            .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "PelneImieNazwisko", nowaObecnosc.UczenId);
        return View(nowaObecnosc);
    }

    public async Task<IActionResult> EdytujObecnosc(int? id)
    {
        if (id == null) return NotFound();

        var obecnosc = await _context.Obecnosci.FindAsync(id);
        if (obecnosc == null) return NotFound();

        var uczniowie = _context.Uczniowie
            .Select(u => new
            {
                u.Id,
                PelneImieNazwisko = u.Imie + " " + u.Nazwisko
            })
            .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "PelneImieNazwisko", obecnosc.UczenId);
        return View(obecnosc);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujObecnosc(int id, Obecnosc obecnoscDoAktualizacji)
    {
        if (id != obecnoscDoAktualizacji.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(obecnoscDoAktualizacji);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Obecnosci));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Obecnosci.Any(o => o.Id == obecnoscDoAktualizacji.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        var uczniowie = _context.Uczniowie
            .Select(u => new
            {
                u.Id,
                PelneImieNazwisko = u.Imie + " " + u.Nazwisko
            })
            .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "PelneImieNazwisko", obecnoscDoAktualizacji.UczenId);
        return View(obecnoscDoAktualizacji);
    }


    public IActionResult Wystawienie()
    {
        var oceny = _context.Oceny
            .Include(o => o.Uczen)
                .ThenInclude(u => u.Klasa)  
            .Include(o => o.Przedmiot)
            .ToList();

        return View(oceny);
    }


    [HttpGet]
    public IActionResult WystawOcene()
    {
        ViewBag.Uczniowie = new SelectList(_context.Uczniowie.Include(u => u.Klasa)
                                    .Select(u => new
                                    {
                                        Id = u.Id,
                                        ImieNazwisko = $"{u.Imie} {u.Nazwisko}" 
                                    }), "Id", "ImieNazwisko");

        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa");
        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa");

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> WystawOcene(Oceny nowaOcena, DateTime DataZajec, bool Obecny)
    {
    
        Console.WriteLine($"UczenId: {nowaOcena.UczenId}, PrzedmiotId: {nowaOcena.PrzedmiotId}, Ocena: {nowaOcena.Ocena}, DataZajec: {DataZajec}, Obecny: {Obecny}");

        if (ModelState.IsValid)
        {
           
            nowaOcena.DataOceny = DateTime.Now;
            _context.Oceny.Add(nowaOcena);
            await _context.SaveChangesAsync();

 
            TempData[$"DataZajec_{nowaOcena.Id}"] = DataZajec.ToShortDateString();

            TempData["Success"] = "Ocena została zapisana.";
            return RedirectToAction(nameof(Wystawienie));
        }


        ViewBag.Uczniowie = new SelectList(_context.Uczniowie.Include(u => u.Klasa)
                                    .Select(u => new
                                    {
                                        Id = u.Id,
                                        ImieNazwisko = $"{u.Imie} {u.Nazwisko}" 
                                    }), "Id", "ImieNazwisko", nowaOcena.UczenId);

        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", nowaOcena.PrzedmiotId);
        ViewBag.Klasy = new SelectList(_context.Klasy, "Id", "Nazwa");

        return View(nowaOcena);
    }

}
