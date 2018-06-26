using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PrijavaController : ApiController
    {

        public Korisnik Get(string id) //Prijava
        {
            Korisnik k = null;//Ovo je samo po sebi validacija
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            
            //Validacija 
            
            if (korisnici.list == null)
                korisnici.list = new Dictionary<string, Korisnik>();

            if (dispeceri.list == null)
                dispeceri.list = new Dictionary<string, Dispecer>();

            if (vozaci.list == null)
                vozaci.list = new Dictionary<string, Vozac>();


            foreach (var kk in korisnici.list)
            {
                if (kk.Value.KorisnickoIme == id)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Musterija;
                    return k;
                }
            }

            foreach (var kk in dispeceri.list)
            {
                if (kk.Value.KorisnickoIme == id)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Dispecer;

                    return k;
                }
            }

            foreach (var kk in vozaci.list)
            {
                if (kk.Value.KorisnickoIme == id)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Vozac;

                    return k;
                }
            }

            return k;
        }


     /*   public Korisnik Put(string id, [FromBody]Korisnik korisnik) //dodela uloge, u zavisnosti od korisnickog imena , to je get ustvari a ne put, ali zbog toga sto id nije int nego string morala sam put metodu
        {
            Korisnik k = null;
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            foreach (var kk in korisnici.list)
            {
                if (kk.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Musterija;
                    return k;
                }
            }

            foreach (var kk in dispeceri.list)
            {
                if (kk.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Dispecer;

                    return k;
                }
            }

            foreach (var kk in vozaci.list)
            {
                if (kk.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Vozac;

                    return k;
                }
            }

            return k;
        }
        */

        public string Post([FromBody]Korisnik korisnik) // Prijava , autentifikacija, index str , moram da posaljem username i pass i zbog toga moram post metodu da koristim
        {
            //Validacija
            if (korisnik == null)
                return "Neuspesna prijava";

            if (String.IsNullOrEmpty(korisnik.KorisnickoIme) || String.IsNullOrEmpty(korisnik.Lozinka))
                return "Neuspesna prijava";


            Regex korImeReg = new Regex("[0-9a-zA-Z]{4,}");
            Regex lozinkaReg = new Regex("[0-9a-zA-Z]{8,}");

            if (!korImeReg.IsMatch(korisnik.KorisnickoIme) || !lozinkaReg.IsMatch(korisnik.Lozinka))
                return "Neuspesna prijava";

            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];

            //Validacija 
            if (korisnici.list == null)
                korisnici.list = new Dictionary<string, Korisnik>();

            if (dispeceri.list == null)
                dispeceri.list = new Dictionary<string, Dispecer>();

            if (vozaci.list == null)
                vozaci.list = new Dictionary<string, Vozac>();



            foreach (var k in korisnici.list)
            {
                if ((k.Value.KorisnickoIme.Equals(korisnik.KorisnickoIme)) && (k.Value.Lozinka.Equals(korisnik.Lozinka)))
                {
                    if (k.Value.Ban == 0)
                    {
                        return "Uspesno";
                    }
                    else
                    {
                        return "Banovan si";
                    }

                }
            }

                foreach (var k in dispeceri.list){
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme && k.Value.Lozinka == korisnik.Lozinka){
                    if (k.Value.Ban == 0)
                    {
                        return "Uspesno";
                    }
                    else
                    {
                        return "Banovan si";
                    }

                }
            }

                foreach (var k in vozaci.list)
                {
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme && k.Value.Lozinka == korisnik.Lozinka)
                {
                    if (k.Value.Ban == 0)
                    {
                        return "Uspesno";
                    }
                    else
                    {
                        return "Banovan si";
                    }

                }
            }

                return "Neuspesna prijava";
            }
        
        
        }
    }
