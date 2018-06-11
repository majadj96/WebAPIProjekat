using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WebAPI.Models.Enums.Enumss;

namespace WebAPI.Models
{
  


    public class Korisnik
    {
        public string Id { get; set; }
        public string KorisnickoIme { get; set; } // ID
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Pol Pol { get; set; }
        public string JMBG { get; set; }
        public string KontaktTelefon { get; set; }
        public string Email { get; set; }
        public Uloga Uloga { get; set; }
        public Korisnik() { }

        public Korisnik(string id, string ime, string prezime, string korisnickoIme, string lozinka, string jmbg, string kontakt, string pol, string email) : this()
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            JMBG = jmbg;
            KontaktTelefon = kontakt;
            if (pol.Equals("Muski")) { Pol = Pol.Muski; } else { Pol = Pol.Zenski; }
            Email = email;
        }

    }

}