using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Vozac : Korisnik
    {
        public Lokacija Lokacija { get; set; }
        public Automobil Automobil { get; set; }

        public Vozac() { }
       

        public Vozac(string id, string ime, string prezime, string korisnickoIme, string lozinka, string jmbg, string kontakt, string pol, 
            string email,double x, double y, string ulicaBroj,string mesto,string zip,string brojAuta, int godisteAuta, string registracijaAuta
            ,string tipAuta) : this()
        {
            //Licne INFO
            Id = id;
            Ime = ime;
            Prezime = prezime;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            JMBG = jmbg;
            KontaktTelefon = kontakt;
            if (pol.Equals("Muski")) { Pol = Enums.Enumss.Pol.Muski; } else { Pol = Enums.Enumss.Pol.Zenski; }
            Email = email;

            //LOKACIJA
            Lokacija l = new Lokacija();
            l.X = x; l.Y = y;
            Adresa a = new Adresa(); // Treba za lokaciju
            a.UlicaBroj = ulicaBroj; a.NaseljenoMesto = mesto; a.PozivniBrojMesta = zip;
            l.Adresa = a;
            Lokacija = l;
            
            //AUTOMOBIL
            Automobil auto = new Automobil();
            auto.Broj = brojAuta;
            auto.Godiste = godisteAuta;
            auto.Registracija = registracijaAuta;
            if (tipAuta.Equals("Putnicki")){ auto.Tip = Enums.Enumss.TipAuta.Putnicki; } else if (tipAuta.Equals("Kombi")){ auto.Tip = Enums.Enumss.TipAuta.Kombi; };
            Automobil = auto;
        }


    }
}