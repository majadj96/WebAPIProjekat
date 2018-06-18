using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DispecerController : ApiController
    {
        //Formiranje voznje

        public bool Post([FromBody]Voznja voznja)
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];


            string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt";
            StringBuilder sb = new StringBuilder();
            voznja.Id = voznje.list.Count.ToString();
            voznja.DatumVreme = DateTime.Now;
            voznja.StatusVoznje = Models.Enums.Enumss.StatusVoznje.Formirana;
            voznja.idKorisnik = " ";
            //kreira se id vozaca
            //voznja.idVozac
            bool zauzetVozac = false;
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            foreach(var vv in vozaci.list)
            {
                if (vv.Value.Zauzet == 0 && vv.Value.Automobil.Tip==voznja.Automobil)
                {
                    zauzetVozac = true;
                    vv.Value.Zauzet = 1;
                    voznja.idVozac = vv.Value.Id;

                    //Update zauzetog vozaca
                    var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt");
                    lines[int.Parse(vv.Key)] = vv.Value.Id + ";" + vv.Value.Ime + ";" + vv.Value.Prezime + ";" + vv.Value.KorisnickoIme + ";" + vv.Value.Lozinka + ";" + vv.Value.JMBG + ";" + vv.Value.KontaktTelefon + ";" + vv.Value.Pol + ";" + vv.Value.Email + ";" + vv.Value.Lokacija.X + ";" + vv.Value.Lokacija.Y + ";" + vv.Value.Lokacija.Adresa.UlicaBroj + ";" + vv.Value.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Value.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Value.Automobil.Broj + ";" + vv.Value.Automobil.Godiste + ";" + vv.Value.Automobil.Registracija + ";" + vv.Value.Automobil.Tip + ";" + vv.Value.Zauzet;
                    File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt", lines);

                    vozaci = new Vozaci("~/App_Data/vozaci.txt");
                    HttpContext.Current.Application["vozaci"] = vozaci;

                    break;
                }


            }
            if (zauzetVozac == false)
            {
                return false;
            }
            voznja.Komentar = new Komentar();
            voznja.Odrediste = new Lokacija();
            voznja.Odrediste.Adresa = new Adresa();

            voznja.Komentar.Ocena = " ";
            voznja.Komentar.Opis = " ";
            voznja.Komentar.Id = " ";
            voznja.Komentar.idVoznja = " ";
            voznja.Komentar.idKorisnik = " ";
            voznja.idKorisnik = " ";
            voznja.Odrediste.X = 0;
            voznja.Odrediste.Y = 0;
            voznja.Odrediste.Adresa.UlicaBroj = " ";
            voznja.Odrediste.Adresa.PozivniBrojMesta = " ";
            voznja.Odrediste.Adresa.NaseljenoMesto = " ";
            voznja.Id = voznje.list.Count.ToString();

            sb.Append(voznja.Id + ";" + voznja.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voznja.Lokacija.X + ";" + voznja.Lokacija.Y + ";" + voznja.Lokacija.Adresa.UlicaBroj + ";" + voznja.Lokacija.Adresa.NaseljenoMesto + ";" + voznja.Lokacija.Adresa.PozivniBrojMesta + ";" + voznja.Automobil + ";" + voznja.idKorisnik + ";" + voznja.Odrediste.X + ";" + voznja.Odrediste.Y + ";" + voznja.Odrediste.Adresa.UlicaBroj + ";" + voznja.Odrediste.Adresa.NaseljenoMesto + ";" + voznja.Odrediste.Adresa.PozivniBrojMesta + ";" + voznja.idDispecer + ";" + voznja.idVozac + ";" + voznja.Iznos + ";" + voznja.Komentar.Opis + ";" + " " + ";" + voznja.Komentar.idKorisnik + ";" + voznja.Komentar.idVoznja + ";" + voznja.Komentar.Ocena + ";" + voznja.StatusVoznje + "\n");

            if (!File.Exists(path))
                File.WriteAllText(path, sb.ToString());
            else
                File.AppendAllText(path, sb.ToString());

            voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;
            return true;

        }

        //Obradjivanje voznje

        public bool Put(string id,[FromBody]Voznja voznja)//menjam dispecera i dodeljujem mu voznju
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            Vozac vv = vozaci.list[voznja.idVozac];

            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            Voznja voki = voznje.list[voznja.Id];

        
            voki.Automobil = vv.Automobil.Tip;
            voki.idDispecer = voznja.idDispecer;
            voki.idVozac = voznja.idVozac;
            voki.StatusVoznje = Models.Enums.Enumss.StatusVoznje.Obradjena;

            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt");
            lines[int.Parse(voki.Id)] = voki.Id + ";" + voki.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voki.Lokacija.X + ";" + voki.Lokacija.Y + ";" + voki.Lokacija.Adresa.UlicaBroj + ";" + voki.Lokacija.Adresa.NaseljenoMesto + ";" + voki.Lokacija.Adresa.PozivniBrojMesta + ";" + voki.Automobil + ";" + voki.idKorisnik + ";" + voki.Odrediste.X + ";" + voki.Odrediste.Y + ";" + voki.Odrediste.Adresa.UlicaBroj + ";" + voki.Odrediste.Adresa.NaseljenoMesto + ";" + voki.Odrediste.Adresa.PozivniBrojMesta + ";" + voki.idDispecer + ";" + voki.idVozac + ";" + voki.Iznos + ";" + voki.Komentar.Opis + ";" + voki.Komentar.DatumObjave + ";" + voki.Komentar.idKorisnik + ";" + voki.Komentar.idVoznja + ";" + voki.Komentar.Ocena + ";" + voki.StatusVoznje;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt", lines);

            voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;
          
            //Sada vozaca
          
            vv.Zauzet = 1;

            lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt");
            lines[int.Parse(vv.Id)] = vv.Id + ";" + vv.Ime + ";" + vv.Prezime + ";" + vv.KorisnickoIme + ";" + vv.Lozinka + ";" + vv.JMBG + ";" + vv.KontaktTelefon + ";" + vv.Pol + ";" + vv.Email + ";" + vv.Lokacija.X + ";" + vv.Lokacija.Y + ";" + vv.Lokacija.Adresa.UlicaBroj + ";" + vv.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Automobil.Broj + ";" + vv.Automobil.Godiste + ";" + vv.Automobil.Registracija + ";" + vv.Automobil.Tip + ";" + vv.Zauzet;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt", lines);

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;
            

            return true;
        }

   

        public List<Voznja> Get()
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];
            List<Voznja> kreiraneVoznje = new List<Voznja>();
            foreach (var v in voznje.list)
            {

                if (v.Value.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Kreirana)
                {
                    kreiraneVoznje.Add(v.Value);
                }

            }


            return kreiraneVoznje;
        }



    }
}
