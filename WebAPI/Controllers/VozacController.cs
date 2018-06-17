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
using WebAPI.Models.Enums;

namespace WebAPI.Controllers
{
    public class VozacController : ApiController
    {
        //put za uspesno

        public bool Delete(string id)//ovo je id voznje koja se stavlja na neuspesna
        {
            string[] ids = id.Split(';');

            Voznje voznje1 = (Voznje)HttpContext.Current.Application["voznje"];//sve voznje
            string id1 = ids[0];//voznje
            string id2 = ids[1];//vozaca
            Voznja voznja = null;
            foreach (var v in voznje1.list)
            {
                if (v.Value.Id == id1)
                {
                    voznja = v.Value;
                    if (voznja.StatusVoznje == Enumss.StatusVoznje.Neuspesna || voznja.StatusVoznje == Enumss.StatusVoznje.Otkazana || voznja.StatusVoznje == Enumss.StatusVoznje.Kreirana || voznja.StatusVoznje == Enumss.StatusVoznje.Uspesna)
                    {
                        return false; //voznja je vec otkazana,uspesna,kreirana ili otkazana
                    }
                    break;
                }
            }
           
            voznja.StatusVoznje = Enumss.StatusVoznje.Neuspesna;

            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt");
            lines[int.Parse(id1)] = voznja.Id + ";" + voznja.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voznja.Lokacija.X + ";" + voznja.Lokacija.Y + ";" + voznja.Lokacija.Adresa.UlicaBroj + ";" + voznja.Lokacija.Adresa.NaseljenoMesto + ";" + voznja.Lokacija.Adresa.PozivniBrojMesta + ";" + voznja.Automobil + ";" + voznja.idKorisnik + ";" + voznja.Odrediste.X + ";" + voznja.Odrediste.Y + ";" + voznja.Odrediste.Adresa.UlicaBroj + ";" + voznja.Odrediste.Adresa.NaseljenoMesto + ";" + voznja.Odrediste.Adresa.PozivniBrojMesta + ";" + voznja.idDispecer + ";" + voznja.idVozac + ";" + voznja.Iznos + ";" + voznja.Komentar.Opis + ";" + voznja.Komentar.DatumObjave + ";" + voznja.Komentar.idKorisnik + ";" + voznja.Komentar.idVoznja + ";" + voznja.Komentar.Ocena + ";" + voznja.StatusVoznje;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt", lines);


            Voznje voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;

            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];//sve voznje

            Vozac vv = vozaci.list[id2];
            vv.Zauzet = 0;

            lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt");
            lines[int.Parse(id2)] = vv.Id + ";" + vv.Ime + ";" + vv.Prezime + ";" + vv.KorisnickoIme + ";" + vv.Lozinka + ";" + vv.JMBG + ";" + vv.KontaktTelefon + ";" + vv.Pol + ";" + vv.Email + ";" + vv.Lokacija.X + ";" + vv.Lokacija.Y + ";" + vv.Lokacija.Adresa.UlicaBroj + ";" + vv.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Automobil.Broj + ";" + vv.Automobil.Godiste + ";" + vv.Automobil.Registracija + ";" + vv.Automobil.Tip + ";" + vv.Zauzet;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt", lines);

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;



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
                    if (v.Value.Zauzet == 0)
                    {
                        listaSlobodnih.Add(v.Value);
                    }

                }
            }

            return listaSlobodnih;

        }
    }
}
