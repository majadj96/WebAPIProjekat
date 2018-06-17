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
    public class KorisnikController : ApiController
    {


        public Korisnik Put(string id, [FromBody]Korisnik korisnik)
        {
            //lalala
            string[] korisnickaImena = korisnik.KorisnickoIme.Split(';');

            string stariUser = korisnickaImena[0];
            string noviUser = korisnickaImena[1];

            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];



            bool dispecer = false;
            bool musterija = false;
            bool vozacB = false;
            Korisnik kmaja = new Korisnik();
            Vozac vozac = new Vozac();

            foreach (var k in vozaci.list)
            {
                if (k.Value.KorisnickoIme == noviUser)
                {
                    if (stariUser == noviUser)
                    {
                        id = k.Value.Id;
                        kmaja = k.Value;
                        vozacB = true;
                        vozac = k.Value;
                        break;
                    }
                    //vec postoji ! greska!
                    kmaja.KorisnickoIme = "postoji";
                    return kmaja;
                }
            }
            foreach (var k in vozaci.list)
            {
                if (k.Value.KorisnickoIme == stariUser)
                {
                    id = k.Value.Id;
                    kmaja = k.Value;
                    vozacB = true;
                    vozac = k.Value;
                    break;
                }
            }





            foreach (var k in dispeceri.list)
            {
                if (k.Value.KorisnickoIme == noviUser)
                {
                    if (stariUser == noviUser)
                    {
                        id = k.Value.Id;
                        kmaja = k.Value;
                        dispecer = true;
                        break;
                    }
                    //vec postoji ! greska!
                    kmaja.KorisnickoIme = "postoji";
                    return kmaja;
                }
            }
            foreach (var k in dispeceri.list)
            {
                if (k.Value.KorisnickoIme == stariUser)
                {
                    id = k.Value.Id;
                    kmaja = k.Value;
                    dispecer = true;
                    break;
                }
            }


            foreach (var k in korisnici.list)
            {

                if (k.Value.KorisnickoIme == noviUser)
                {
                    if (stariUser == noviUser)
                    {
                        id = k.Value.Id;
                        kmaja = k.Value;
                        musterija = true;
                        break;
                    }

                    //vec postoji ! greska!
                    kmaja.KorisnickoIme = "postoji";
                    return kmaja;
                }
            }

            foreach (var k in korisnici.list)
            {
                if (k.Value.KorisnickoIme == stariUser)
                {
                    id = k.Value.Id;
                    kmaja = k.Value;
                    musterija = true;
                    break;
                }
            }
            kmaja.KorisnickoIme = noviUser;
            kmaja.Lozinka = korisnik.Lozinka;
            kmaja.KontaktTelefon = korisnik.KontaktTelefon;
            kmaja.Prezime = korisnik.Prezime;
            kmaja.Ime = korisnik.Ime;
            kmaja.Email = korisnik.Email;



            if (musterija)
            {
                string path = "~/App_Data/korisnici.txt";
                path = HostingEnvironment.MapPath(path);

                var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt");
                korisnik = kmaja;
                lines[int.Parse(id)] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email;
                File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt", lines);

                korisnici = new Korisnici("~/App_Data/korisnici.txt");
                HttpContext.Current.Application["korisnici"] = korisnici;
            }
            if (dispecer)
            {
                string path = "~/App_Data/dispeceri.txt";
                path = HostingEnvironment.MapPath(path);

                var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\dispeceri.txt");
                korisnik = kmaja;
                lines[int.Parse(id)] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email;
                File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\dispeceri.txt", lines);

                dispeceri = new Dispeceri("~/App_Data/dispeceri.txt");
                HttpContext.Current.Application["dispeceri"] = dispeceri;



            }

            if (vozacB)
            {
                string path = "~/App_Data/vozaci.txt";
                path = HostingEnvironment.MapPath(path);

                var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt");
                korisnik = kmaja;

                vozac.KorisnickoIme = korisnik.KorisnickoIme;
                vozac.Lozinka = korisnik.Lozinka;
                vozac.Ime = korisnik.Ime;
                vozac.Prezime = korisnik.Prezime;
                vozac.KontaktTelefon = korisnik.KontaktTelefon;
                vozac.Email = korisnik.Email;



                lines[int.Parse(id)] = vozac.Id + ";" + vozac.Ime + ";" + vozac.Prezime + ";" + vozac.KorisnickoIme + ";" + vozac.Lozinka + ";" + vozac.JMBG + ";" + vozac.KontaktTelefon + ";" + vozac.Pol + ";" + vozac.Email + ";" + vozac.Lokacija.X + ";" + vozac.Lokacija.Y + ";" + vozac.Lokacija.Adresa.UlicaBroj + ";" + vozac.Lokacija.Adresa.NaseljenoMesto + ";" + vozac.Lokacija.Adresa.PozivniBrojMesta + ";" + vozac.Automobil.Broj + ";" + vozac.Automobil.Godiste + ";" + vozac.Automobil.Registracija + ";" + vozac.Automobil.Tip;
                File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt", lines);

                vozaci = new Vozaci("~/App_Data/vozaci.txt");
                HttpContext.Current.Application["vozaci"] = vozaci;



            }

            return korisnik;
        }


        public bool Post([FromBody]Korisnik korisnik)
        {

            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];


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


            if (!postoji)
            {
                string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt";
                StringBuilder sb = new StringBuilder();
                korisnik.Id = korisnici.list.Count.ToString();
                sb.Append(korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email + "\n");

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
                //Postoji korisnik
            }
        }


    }
}
