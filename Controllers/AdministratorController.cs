using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dziennik_elektroniczny.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "Administrator")]
public class AdministratorController : Controller
{
    private readonly DziennikContext _context;

    public AdministratorController(DziennikContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard() => View();

 
    public async Task<IActionResult> Uczniowie()
    {
        var uczniowie = await _context.Uczniowie.Include(u => u.Klasa).ToListAsync();
        return View(uczniowie);
    }

    public IActionResult DodajUcznia()
    {
        if (!_context.Klasy.Any())
        {
            TempData["Error"] = "Najpierw dodaj klasy, zanim dodasz ucznia.";
            return RedirectToAction(nameof(Klasy));
        }

        ViewData["Klasy"] = new SelectList(_context.Klasy, "Id", "Nazwa");
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

        ViewData["Klasy"] = new SelectList(_context.Klasy, "Id", "Nazwa", uczen.KlasaId);
        return View(uczen);
    }

    public async Task<IActionResult> EdytujUcznia(int? id)
    {
        if (id == null) return NotFound();

        var uczen = await _context.Uczniowie.FindAsync(id);
        if (uczen == null) return NotFound();

        ViewData["Klasy"] = new SelectList(_context.Klasy, "Id", "Nazwa", uczen.KlasaId);
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
                if (!UczenExists(uczen.Id)) return NotFound();
                throw;
            }
        }

        ViewData["Klasy"] = new SelectList(_context.Klasy, "Id", "Nazwa", uczen.KlasaId);
        return View(uczen);
    }

    public async Task<IActionResult> UsunUcznia(int? id)
    {
        if (id == null) return NotFound();

        var uczen = await _context.Uczniowie.Include(u => u.Klasa).FirstOrDefaultAsync(u => u.Id == id);
        if (uczen == null) return NotFound();

        return View(uczen);
    }

    [HttpPost, ActionName("UsunUcznia")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PotwierdzUsuniecieUcznia(int id)
    {
        var uczen = await _context.Uczniowie.FindAsync(id);
        if (uczen != null)
        {
            _context.Uczniowie.Remove(uczen);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Uczniowie));
    }


    public async Task<IActionResult> Klasy()
    {
        var klasy = await _context.Klasy.Include(k => k.Uczniowie).ToListAsync();
        return View(klasy);
    }

    public IActionResult DodajKlase() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajKlase(Klasa klasa)
    {
        if (ModelState.IsValid)
        {
            _context.Klasy.Add(klasa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Klasy));
        }
        return View(klasa);
    }

    public async Task<IActionResult> EdytujKlase(int? id)
    {
        if (id == null) return NotFound();

        var klasa = await _context.Klasy.FindAsync(id);
        if (klasa == null) return NotFound();

        return View(klasa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujKlase(int id, Klasa klasa)
    {
        if (id != klasa.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(klasa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Klasy));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Klasy.Any(k => k.Id == klasa.Id)) return NotFound();
                throw;
            }
        }

        return View(klasa);
    }

    public async Task<IActionResult> UsunKlase(int? id)
    {
        if (id == null) return NotFound();

        var klasa = await _context.Klasy.Include(k => k.Uczniowie).FirstOrDefaultAsync(k => k.Id == id);
        if (klasa == null) return NotFound();

        if (klasa.Uczniowie.Any())
        {
            TempData["Error"] = "Nie można usunąć klasy, która ma przypisanych uczniów.";
            return RedirectToAction(nameof(Klasy));
        }

        return View(klasa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsunKlase(int id)
    {
        var klasa = await _context.Klasy.FindAsync(id);
        if (klasa != null)
        {
            _context.Klasy.Remove(klasa);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Klasy));
    }

    public async Task<IActionResult> Nauczyciele()
    {
        var nauczyciele = await _context.Nauczyciele.ToListAsync();
        return View(nauczyciele);
    }

    public IActionResult DodajNauczyciela() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajNauczyciela(Nauczyciel nauczyciel)
    {
        if (ModelState.IsValid)
        {
            _context.Nauczyciele.Add(nauczyciel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Nauczyciele));
        }
        return View(nauczyciel);
    }

    public async Task<IActionResult> EdytujNauczyciela(int? id)
    {
        if (id == null) return NotFound();

        var nauczyciel = await _context.Nauczyciele.FindAsync(id);
        if (nauczyciel == null) return NotFound();

        return View(nauczyciel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujNauczyciela(int id, Nauczyciel nauczyciel)
    {
        if (id != nauczyciel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nauczyciel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Nauczyciele));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Nauczyciele.Any(n => n.Id == nauczyciel.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        return View(nauczyciel);
    }

    public async Task<IActionResult> UsunNauczyciela(int? id)
    {
        if (id == null) return NotFound();

        var nauczyciel = await _context.Nauczyciele.FirstOrDefaultAsync(n => n.Id == id);
        if (nauczyciel == null) return NotFound();

        return View(nauczyciel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsunNauczyciela(int id)
    {
        var nauczyciel = await _context.Nauczyciele.FindAsync(id);
        if (nauczyciel != null)
        {
            _context.Nauczyciele.Remove(nauczyciel);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Nauczyciele));
    }

    private bool UczenExists(int id) => _context.Uczniowie.Any(e => e.Id == id);

    public async Task<IActionResult> Przedmioty()
    {
        var przedmioty = await _context.Przedmioty.Include(p => p.Nauczyciel).ToListAsync();
        return View(przedmioty);
    }

    public async Task<IActionResult> DodajPrzedmiot()
    {
        if (!_context.Nauczyciele.Any())
        {
            TempData["Error"] = "Najpierw dodaj nauczycieli, zanim dodasz przedmiot.";
            return RedirectToAction(nameof(Nauczyciele));
        }

        var nauczyciele = await _context.Nauczyciele
                                         .Select(n => new
                                         {
                                             n.Id,
                                             ImieNazwisko = $"{n.Imie} {n.Nazwisko}"
                                         })
                                         .ToListAsync();

        ViewData["Nauczyciele"] = new SelectList(nauczyciele, "Id", "ImieNazwisko");
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

        var nauczyciele = await _context.Nauczyciele
                                         .Select(n => new
                                         {
                                             n.Id,
                                             ImieNazwisko = $"{n.Imie} {n.Nazwisko}"
                                         })
                                         .ToListAsync();


        ViewData["Nauczyciele"] = new SelectList(nauczyciele, "Id", "ImieNazwisko", przedmiot.NauczycielId);
        return View(przedmiot);
    }
    public async Task<IActionResult> EdytujPrzedmiot(int? id)
    {
        if (id == null) return NotFound();

        var przedmiot = await _context.Przedmioty.Include(p => p.Nauczyciel).FirstOrDefaultAsync(p => p.Id == id);
        if (przedmiot == null) return NotFound();


        var nauczyciele = await _context.Nauczyciele
                                         .Select(n => new
                                         {
                                             n.Id,
                                             ImieNazwisko = $"{n.Imie} {n.Nazwisko}"
                                         })
                                         .ToListAsync();

        ViewData["Nauczyciele"] = new SelectList(nauczyciele, "Id", "ImieNazwisko", przedmiot.NauczycielId);
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

        var nauczyciele = await _context.Nauczyciele
                                         .Select(n => new
                                         {
                                             n.Id,
                                             ImieNazwisko = $"{n.Imie} {n.Nazwisko}"
                                         })
                                         .ToListAsync();

        ViewData["Nauczyciele"] = new SelectList(nauczyciele, "Id", "ImieNazwisko", przedmiot.NauczycielId);
        return View(przedmiot);
    }

    public async Task<IActionResult> UsunPrzedmiot(int? id)
    {
        if (id == null) return NotFound();

        var przedmiot = await _context.Przedmioty.Include(p => p.Nauczyciel).FirstOrDefaultAsync(p => p.Id == id);
        if (przedmiot == null) return NotFound();

        return View(przedmiot);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsunPrzedmiot(int id)
    {
        var przedmiot = await _context.Przedmioty.FindAsync(id);
        if (przedmiot != null)
        {
            _context.Przedmioty.Remove(przedmiot);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Przedmioty));
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

    public async Task<IActionResult> UsunZajecia(int? id)
    {
        if (id == null) return NotFound();

        var zajecia = await _context.Zajecia
            .Include(z => z.Klasa)
            .Include(z => z.Przedmiot)
            .FirstOrDefaultAsync(z => z.Id == id);
        if (zajecia == null) return NotFound();

        return View(zajecia);
    }

    [HttpPost, ActionName("UsunZajecia")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PotwierdzUsuniecieZajecia(int id)
    {
        var zajecia = await _context.Zajecia.FindAsync(id);
        if (zajecia != null)
        {
            _context.Zajecia.Remove(zajecia);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Zajecia));
    }

    public async Task<IActionResult> Obecnosci()
    {
        var obecnosci = await _context.Obecnosci
            .Include(o => o.Uczen) 
            .ToListAsync();

        
        return View(obecnosci);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajObecnosc(Obecnosc obecnosc)
    {
       
        if (!_context.Uczniowie.Any(u => u.Id == obecnosc.UczenId))
        {
            ModelState.AddModelError("UczenId", "Wybrany uczeń nie istnieje.");
        }

        if (obecnosc.DataZajec == DateTime.MinValue)
        {
            ModelState.AddModelError("DataZajec", "Data zajęć jest wymagana.");
        }

        if (ModelState.IsValid)
        {
            _context.Obecnosci.Add(obecnosc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Obecnosci));
        }

     
        ViewBag.Uczniowie = new SelectList(_context.Uczniowie, "Id", "ImieNazwisko", obecnosc.UczenId);
        return View(obecnosc);
    }

    public IActionResult DodajObecnosc()
    {
        var uczniowie = _context.Uczniowie
                                .Select(u => new
                                {
                                    u.Id,
                                    ImieNazwisko = u.Imie + " " + u.Nazwisko
                                })
                                .ToList();

     
        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "ImieNazwisko");
        return View();
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
                                    ImieNazwisko = u.Imie + " " + u.Nazwisko
                                })
                                .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "ImieNazwisko", obecnosc.UczenId);
        return View(obecnosc);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujObecnosc(int id, Obecnosc obecnosc)
    {
        if (id != obecnosc.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(obecnosc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Obecnosci));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Obecnosci.Any(o => o.Id == obecnosc.Id)) return NotFound();
                throw;
            }
        }

        ViewBag.Uczniowie = new SelectList(_context.Uczniowie, "Id", "ImieNazwisko", obecnosc.UczenId);
        return View(obecnosc);
    }

    public async Task<IActionResult> UsunObecnosc(int? id)
    {
        if (id == null) return NotFound();

        var obecnosc = await _context.Obecnosci
            .Include(o => o.Uczen)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (obecnosc == null) return NotFound();
        return View(obecnosc);
    }

    [HttpPost, ActionName("UsunObecnosc")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PotwierdzUsuniecieObecnosci(int id)
    {
        var obecnosc = await _context.Obecnosci.FindAsync(id);
        if (obecnosc != null)
        {
            _context.Obecnosci.Remove(obecnosc);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Obecnosci));
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

        var uczniowie = _context.Uczniowie
            .Select(u => new
            {
                u.Id,
                ImieNazwisko = u.Imie + " " + u.Nazwisko
            })
            .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "ImieNazwisko");
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa");

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DodajOcene(Oceny nowaOcena)
    {
      
        if (!_context.Uczniowie.Any(u => u.Id == nowaOcena.UczenId))
        {
            ModelState.AddModelError("UczenId", "Wybrany uczeń nie istnieje.");
        }

        if (!_context.Przedmioty.Any(p => p.Id == nowaOcena.PrzedmiotId))
        {
            ModelState.AddModelError("PrzedmiotId", "Wybrany przedmiot nie istnieje.");
        }

        if (ModelState.IsValid)
        {
            _context.Oceny.Add(nowaOcena);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Oceny));
        }

        var uczniowie = _context.Uczniowie
            .Select(u => new
            {
                u.Id,
                ImieNazwisko = u.Imie + " " + u.Nazwisko
            })
            .ToList();

        ViewBag.Uczniowie = new SelectList(uczniowie, "Id", "ImieNazwisko", nowaOcena.UczenId);
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", nowaOcena.PrzedmiotId);

        return View(nowaOcena);
    }


    public async Task<IActionResult> EdytujOcene(int? id)
    {
        if (id == null) return NotFound();

        var ocena = await _context.Oceny.FindAsync(id);
        if (ocena == null) return NotFound();

        ViewBag.Uczniowie = new SelectList(_context.Uczniowie, "Id", "Nazwisko", ocena.UczenId);
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", ocena.PrzedmiotId);

        return View(ocena);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EdytujOcene(int id, Oceny aktualnaOcena)
    {
        if (id != aktualnaOcena.Id) return NotFound();

   
        if (!_context.Uczniowie.Any(u => u.Id == aktualnaOcena.UczenId))
        {
            ModelState.AddModelError("UczenId", "Wybrany uczeń nie istnieje.");
        }

        if (!_context.Przedmioty.Any(p => p.Id == aktualnaOcena.PrzedmiotId))
        {
            ModelState.AddModelError("PrzedmiotId", "Wybrany przedmiot nie istnieje.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(aktualnaOcena);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Oceny));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Oceny.Any(o => o.Id == aktualnaOcena.Id)) return NotFound();
                throw;
            }
        }

        ViewBag.Uczniowie = new SelectList(_context.Uczniowie, "Id", "Nazwisko", aktualnaOcena.UczenId);
        ViewBag.Przedmioty = new SelectList(_context.Przedmioty, "Id", "Nazwa", aktualnaOcena.PrzedmiotId);
        return View(aktualnaOcena);
    }


    public async Task<IActionResult> UsunOcene(int? id)
    {
        if (id == null) return NotFound();

        var ocena = await _context.Oceny
            .Include(o => o.Uczen)
            .Include(o => o.Przedmiot)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (ocena == null) return NotFound();

        return View(ocena);
    }

    [HttpPost, ActionName("UsunOcene")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PotwierdzUsuniecieOceny(int id)
    {
        var ocena = await _context.Oceny.FindAsync(id);
        if (ocena != null)
        {
            _context.Oceny.Remove(ocena);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Oceny));
    }
}
