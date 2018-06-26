using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class KorisnikController : ApiController // imam put i post za sada
    {
        public List<Korisnik> Get()//Koristi se a u musteriji ne
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            List<Korisnik> korisnicici = new List<Korisnik>();

            //Validacija
            if (korisnici.list == null)
                korisnici.list = new Dictionary<string, Korisnik>();

            foreach (var k in korisnici.list)
                korisnicici.Add(k.Value);

            return korisnicici;

        }

        public Korisnik Get(string id)
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            //Validacija
            if(int.Parse(id)>=0 && int.Parse(id) < korisnici.list.Count)
            {
                return korisnici.list[id];
            }
            else
            {
                return null;
            }

        }
        
        public bool Put(string id, [FromBody]Korisnik korisnik) // Izmena za sve korisnike(musterija,vozac,admin)
        {
            #region VALIDACIJA
            if (korisnik == null)
                return false;

            if (String.IsNullOrEmpty(korisnik.KorisnickoIme) || String.IsNullOrEmpty(korisnik.Lozinka) || String.IsNullOrEmpty(korisnik.Ime) || String.IsNullOrEmpty(korisnik.Prezime) || String.IsNullOrEmpty(korisnik.KontaktTelefon) || String.IsNullOrEmpty(korisnik.Email))
                return false;

            Regex telefonReg = new Regex("[0-9]{6,14}");
            Regex emailReg = new Regex(@"[a-z0-9._% +-]+@[a-z0-9.-]+\.[a-z]{2,3}$");
            Regex korImeReg = new Regex("[0-9a-zA-Z]{4,}");
            Regex lozinkaReg = new Regex("[0-9a-zA-Z]{8,}");

            if (!telefonReg.IsMatch(korisnik.KontaktTelefon) || !emailReg.IsMatch(korisnik.Email) || !korImeReg.IsMatch(korisnik.KorisnickoIme) || !lozinkaReg.IsMatch(korisnik.Lozinka))
                return false;
            #endregion


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

            Korisnik korisnikLoc = null;
            Vozac vozacLoc = null;
            Dispecer dispecerLoc = null;

            Korisnik kmaja = new Korisnik(); //korisnik = dispecer
            Vozac vozac = new Vozac(); //on je prosiren


            if (korisnik.Uloga == Models.Enums.Enumss.Uloga.Musterija)
            {
                korisnikLoc = korisnici.list[id]; // to je stari korisnik koga menjam

                //proveri i za vozace i za dispecere
                foreach (var d in dispeceri.list)
                    if (d.Value.KorisnickoIme == korisnik.KorisnickoIme)
                        return false;

                foreach (var v in vozaci.list)
                    if (v.Value.KorisnickoIme == korisnik.KorisnickoIme)
                        return false;

                foreach (var k in korisnici.list)//gledam da li je jedinstven username
                {
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)//ako vec postoji proverim da to nisam ja
                    {
                        if (korisnikLoc.KorisnickoIme == korisnik.KorisnickoIme)//dobro je to je taj user
                        {
                            break;
                        }
                        
                        return false;
                    }
                }

                foreach (var k in korisnici.list)
                {
                    if (k.Value.KorisnickoIme == korisnikLoc.KorisnickoIme)
                    {
                        break;
                    }
                }

                string path = "~/App_Data/korisnici.txt";
                path = HostingEnvironment.MapPath(path);

                var lines = File.ReadAllLines(path);
                korisnikLoc.KorisnickoIme = korisnik.KorisnickoIme;
                korisnikLoc.Lozinka = korisnik.Lozinka;
                korisnikLoc.Email = korisnik.Email;
                korisnikLoc.Ime = korisnik.Ime;
                korisnikLoc.Prezime = korisnik.Prezime;
                korisnikLoc.KontaktTelefon = korisnik.KontaktTelefon;
                korisnik = korisnikLoc;
                lines[int.Parse(id)] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email + ";" + korisnik.Ban;
                File.WriteAllLines(path, lines);

                korisnici = new Korisnici("~/App_Data/korisnici.txt");
                HttpContext.Current.Application["korisnici"] = korisnici;


            }
            else if (korisnik.Uloga == Models.Enums.Enumss.Uloga.Vozac)
            {
                vozacLoc = vozaci.list[id]; // to je stari vozac koga menjam

                foreach (var d in dispeceri.list)
                    if (d.Value.KorisnickoIme == korisnik.KorisnickoIme)
                        return false;

                foreach (var k in korisnici.list)
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                        return false;

                foreach (var k in vozaci.list)//gledam da li je jedinstven username
                {
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)//ako vec postoji proverim da to nisam ja
                    {
                        if (vozacLoc.KorisnickoIme == korisnik.KorisnickoIme)//dobro je to je taj user
                        {
                            break;
                        }

                        return false;
                    }
                }

                foreach (var k in vozaci.list)
                {
                    if (k.Value.KorisnickoIme == vozacLoc.KorisnickoIme)
                    {
                        break;
                    }
                }

                string path = "~/App_Data/vozaci.txt";
                path = HostingEnvironment.MapPath(path);

                var lines = File.ReadAllLines(path);

                vozacLoc.KorisnickoIme = korisnik.KorisnickoIme;
                vozacLoc.Lozinka = korisnik.Lozinka;
                vozacLoc.Ime = korisnik.Ime;
                vozacLoc.Prezime = korisnik.Prezime;
                vozacLoc.KontaktTelefon = korisnik.KontaktTelefon;
                vozacLoc.Email = korisnik.Email;
                vozac = vozacLoc;

                lines[int.Parse(id)] = vozac.Id + ";" + vozac.Ime + ";" + vozac.Prezime + ";" + vozac.KorisnickoIme + ";" + vozac.Lozinka + ";" + vozac.JMBG + ";" + vozac.KontaktTelefon + ";" + vozac.Pol + ";" + vozac.Email + ";" + vozac.Lokacija.X + ";" + vozac.Lokacija.Y + ";" + vozac.Lokacija.Adresa.UlicaBroj + ";" + vozac.Lokacija.Adresa.NaseljenoMesto + ";" + vozac.Lokacija.Adresa.PozivniBrojMesta + ";" + vozac.Automobil.Broj + ";" + vozac.Automobil.Godiste + ";" + vozac.Automobil.Registracija + ";" + vozac.Automobil.Tip + ";" + vozac.Zauzet + ";" + vozac.Ban;
                File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt", lines);

                vozaci = new Vozaci("~/App_Data/vozaci.txt");
                HttpContext.Current.Application["vozaci"] = vozaci;

            }
            else if (korisnik.Uloga == Models.Enums.Enumss.Uloga.Dispecer)
            {
                dispecerLoc = dispeceri.list[id]; // to je stari vozac koga menjam

                foreach (var k in korisnici.list)
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                        return false;

                foreach (var k in korisnici.list)
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                        return false;

                foreach (var k in dispeceri.list)//gledam da li je jedinstven username
                {
                    if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)//ako vec postoji proverim da to nisam ja
                    {
                        if (dispecerLoc.KorisnickoIme == korisnik.KorisnickoIme)//dobro je to je taj user
                        {
                            break;
                        }

                        return false;
                    }
                }

                foreach (var k in dispeceri.list)
                {
                    if (k.Value.KorisnickoIme == dispecerLoc.KorisnickoIme)
                    {
                        break;
                    }
                }


                string path = "~/App_Data/dispeceri.txt";
                path = HostingEnvironment.MapPath(path);

                var lines = File.ReadAllLines(path);

                dispecerLoc.KorisnickoIme = korisnik.KorisnickoIme;
                dispecerLoc.Lozinka = korisnik.Lozinka;
                dispecerLoc.Email = korisnik.Email;
                dispecerLoc.Ime = korisnik.Ime;
                dispecerLoc.Prezime = korisnik.Prezime;
                dispecerLoc.KontaktTelefon = korisnik.KontaktTelefon;
                korisnik = dispecerLoc;

                lines[int.Parse(id)] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email;
                File.WriteAllLines(path, lines);

                dispeceri = new Dispeceri("~/App_Data/dispeceri.txt");
                HttpContext.Current.Application["dispeceri"] = dispeceri;


            }

            return true;
        }


        public bool Post([FromBody]Korisnik korisnik) // Registracija
        {
            #region VALIDACIJA
            if (korisnik == null)
                return false;

          if (String.IsNullOrEmpty(korisnik.KorisnickoIme) || String.IsNullOrEmpty(korisnik.Lozinka) || String.IsNullOrEmpty(korisnik.Ime) || String.IsNullOrEmpty(korisnik.Prezime) || String.IsNullOrEmpty((korisnik.Pol).ToString()) || String.IsNullOrEmpty(korisnik.JMBG) || String.IsNullOrEmpty(korisnik.KontaktTelefon) || String.IsNullOrEmpty(korisnik.Email))
                return false;
            
            Regex jmbgReg = new Regex("[0-9]{13}");
            Regex telefonReg = new Regex("[0-9]{6,14}");
            Regex emailReg = new Regex(@"[a-z0-9._% +-]+@[a-z0-9.-]+\.[a-z]{2,3}$");
            Regex korImeReg = new Regex("[0-9a-zA-Z]{4,}");
            Regex lozinkaReg = new Regex("[0-9a-zA-Z]{8,}");
            
            if (!jmbgReg.IsMatch(korisnik.JMBG) || !telefonReg.IsMatch(korisnik.KontaktTelefon) || !emailReg.IsMatch(korisnik.Email) || !korImeReg.IsMatch(korisnik.KorisnickoIme) || !lozinkaReg.IsMatch(korisnik.Lozinka))
                return false;
            #endregion
            

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


            bool postoji = false;

            foreach (var k in dispeceri.list)
            {
                if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    postoji = true;
                    break;
                }
            }

            foreach (var k in korisnici.list)
            {
                if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    postoji = true;
                    break;
                }
            }

            foreach (var k in vozaci.list)
            {
                if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    postoji = true;
                    break;
                }
            }


            if (!postoji)//Ako korisnicko ime nije zauzeto
            {
                string path = "~/App_Data/korisnici.txt";
                path = HostingEnvironment.MapPath(path);

                StringBuilder sb = new StringBuilder();
                korisnik.Id = korisnici.list.Count.ToString();
                korisnik.Ban = 0;
                sb.Append(korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email + ";" + korisnik.Ban + "\n");

                if (!File.Exists(path))
                    File.WriteAllText(path, sb.ToString());
                else
                    File.AppendAllText(path, sb.ToString());

                korisnici = new Korisnici("~/App_Data/korisnici.txt");
                HttpContext.Current.Application["korisnici"] = korisnici;
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
