using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Dispecer : Korisnik
    {
        public Dispecer() { }

        public Dispecer(string id, string ime, string prezime, string korisnickoIme, string lozinka, string jmbg, string kontakt, string pol, string email) : this()
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            JMBG = jmbg;
            KontaktTelefon = kontakt;
           
            if (pol.Equals("Muski")) { Pol = Enums.Enumss.Pol.Muski; } else { Pol = Enums.Enumss.Pol.Zenski; }
            Email = email;
        }
    }
}