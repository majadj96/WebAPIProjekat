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
    public class VozacController : ApiController
    {

        public bool Delete(string id, [FromBody]string idKor)//ovo je id voznje koja se stavlja na neuspesna
        {
            Voznje voznje1 = (Voznje)HttpContext.Current.Application["voznje"];//sve voznje

            Voznja voznja = null;
            foreach (var v in voznje1.list)
            {
                if (v.Value.Id == id)
                {
                    voznja = v.Value;
                    if (voznja.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Neuspesna)
                    {
                        return false; //voznja je vec otkazana
                    }
                    break;
                }
            }
           
            voznja.StatusVoznje = Models.Enums.Enumss.StatusVoznje.Neuspesna;

            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt");
            lines[int.Parse(id)] = voznja.Id + ";" + voznja.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voznja.Lokacija.X + ";" + voznja.Lokacija.Y + ";" + voznja.Lokacija.Adresa.UlicaBroj + ";" + voznja.Lokacija.Adresa.NaseljenoMesto + ";" + voznja.Lokacija.Adresa.PozivniBrojMesta + ";" + voznja.Automobil + ";" + voznja.idKorisnik + ";" + voznja.Odrediste.X + ";" + voznja.Odrediste.Y + ";" + voznja.Odrediste.Adresa.UlicaBroj + ";" + voznja.Odrediste.Adresa.NaseljenoMesto + ";" + voznja.Odrediste.Adresa.PozivniBrojMesta + ";" + voznja.idDispecer + ";" + voznja.idVozac + ";" + voznja.Iznos + ";" + voznja.Komentar.Opis + ";" + voznja.Komentar.DatumObjave + ";" + voznja.Komentar.idKorisnik + ";" + voznja.Komentar.idVoznja + ";" + voznja.Komentar.Ocena + ";" + voznja.StatusVoznje;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt", lines);


            Voznje voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;



            return true;

        }



        public bool Post([FromBody]Vozac vozac)
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            foreach (var v in vozaci.list)
            {
                if (v.Value.KorisnickoIme == vozac.KorisnickoIme)
                    return true;
            }

            string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt";
            StringBuilder sb = new StringBuilder();
            vozac.Id = vozaci.list.Count.ToString();
            vozac.Automobil.Broj = (vozaci.list.Count + 100).ToString();
            sb.Append(vozac.Id + ";" + vozac.Ime + ";" + vozac.Prezime + ";" + vozac.KorisnickoIme + ";" + vozac.Lozinka + ";" + vozac.JMBG + ";" + vozac.KontaktTelefon + ";" + vozac.Pol + ";" + vozac.Email + ";" + vozac.Lokacija.X + ";" + vozac.Lokacija.Y + ";" + vozac.Lokacija.Adresa.UlicaBroj + ";" + vozac.Lokacija.Adresa.NaseljenoMesto + ";" + vozac.Lokacija.Adresa.PozivniBrojMesta + ";" + vozac.Automobil.Broj + ";" + vozac.Automobil.Godiste + ";" + vozac.Automobil.Registracija + ";" + vozac.Automobil.Tip + ";" + vozac.Zauzet + "\n");

            if (!File.Exists(path))
                File.WriteAllText(path, sb.ToString());
            else
                File.AppendAllText(path, sb.ToString());

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;
            return false;
        }
   


        public List<Vozac> Get(string id) //treba da se poklapaju tipovi auta voznje i vozaca
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];
            Voznja voznja = voznje.list[id];
            bool svejedno = false;
            if (voznja.Automobil == Models.Enums.Enumss.TipAuta.Svejedno)
                svejedno = true;


            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            List<Vozac> listaSlobodnih = new List<Vozac>();

            foreach (var v in vozaci.list)
            {
                if (!svejedno)
                {
                    if (voznja.Automobil == v.Value.Automobil.Tip)
                    {
                        if (v.Value.Zauzet == 0)
                        {
                            listaSlobodnih.Add(v.Value);
                        }
                    }
                }
                else
                {
                    listaSlobodnih.Add(v.Value);

                }
            }

            return listaSlobodnih;

        }
    }
}
