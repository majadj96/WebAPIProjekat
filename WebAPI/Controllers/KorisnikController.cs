using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class KorisnikController : ApiController // imam put i post za sada
    {
        public List<Korisnik> Get()
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            List<Korisnik> korisnicici = new List<Korisnik>();
            foreach (var k in korisnici.list)
                korisnicici.Add(k.Value);

            return korisnicici;

        }

        public Korisnik Get(string id)
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            return korisnici.list[id];
        }

        //mozda da saljem korisnik sve novo a id da mi bude od njega i tu imam username , put da vraca false ako ima korisnicko ime

        //u zavisnosti koja je uloga citam iz te aplikacije i onda radim logiku

        public bool Put(string id, [FromBody]Korisnik korisnik) // Izmena ? , treba mi i put da promenim da je banovan
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

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

                var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt");
                korisnikLoc.KorisnickoIme = korisnik.KorisnickoIme;
                korisnikLoc.Lozinka = korisnik.Lozinka;
                korisnikLoc.Email = korisnik.Email;
                korisnikLoc.Ime = korisnik.Ime;
                korisnikLoc.Prezime = korisnik.Prezime;
                korisnikLoc.KontaktTelefon = korisnik.KontaktTelefon;
                korisnik = korisnikLoc;
                lines[int.Parse(id)] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email + ";" + korisnik.Ban;
                File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt", lines);

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

                var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt");

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

                var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\dispeceri.txt");

                dispecerLoc.KorisnickoIme = korisnik.KorisnickoIme;
                dispecerLoc.Lozinka = korisnik.Lozinka;
                dispecerLoc.Email = korisnik.Email;
                dispecerLoc.Ime = korisnik.Ime;
                dispecerLoc.Prezime = korisnik.Prezime;
                dispecerLoc.KontaktTelefon = korisnik.KontaktTelefon;
                korisnik = dispecerLoc;

                lines[int.Parse(id)] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email;
                File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\dispeceri.txt", lines);

                dispeceri = new Dispeceri("~/App_Data/dispeceri.txt");
                HttpContext.Current.Application["dispeceri"] = dispeceri;


            }

            return true;
        }


        public bool Post([FromBody]Korisnik korisnik) // Dodavanje korisnika - DOBRO JE, //validacija
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

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


            if (!postoji)
            {
                string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt";
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
