using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Dziennik_elektroniczny.Models
{
    public class DziennikContext : IdentityDbContext
    {
        public DbSet<Uczen> Uczniowie { get; set; }
        public DbSet<Nauczyciel> Nauczyciele { get; set; }
        public DbSet<Administrator> Administratorzy { get; set; }
        public DbSet<Klasa> Klasy { get; set; }
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<Zajecia> Zajecia { get; set; }
        public DbSet<Obecnosc> Obecnosci { get; set; }
        public DbSet<Oceny> Oceny { get; set; }

        public DziennikContext(DbContextOptions<DziennikContext> options) : base(options)
        {
        }
    }
}




