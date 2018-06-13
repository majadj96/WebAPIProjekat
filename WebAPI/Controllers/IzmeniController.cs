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
    public class IzmeniController : ApiController
    {

        public Korisnik Post([FromBody]Korisnik korisnik)
        {

            string[] korisnickaImena = korisnik.KorisnickoIme.Split(';');

            string stariUser = korisnickaImena[0];
            string noviUser = korisnickaImena[1];
            string id = "";
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];

            bool dispecer = false;
            bool musterija = false;
            bool vozac = false;
            Korisnik kmaja = new Korisnik();

            foreach (var k in dispeceri.list)
            {
                if (k.Value.KorisnickoIme == noviUser)
                {
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

            return korisnik;
        }
    }
    }

