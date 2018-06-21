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



        public bool Put(string id, [FromBody]Vozac vozac)//menjam samo vozaca

        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];//sve voznje
            
            Vozac vv = vozaci.list[id];
            if (vozac.Ban != 2)
            {
                vv.Ban = vozac.Ban;
            }

            if (vozac.Zauzet != 2)
            {
                vv.Zauzet = vozac.Zauzet;
            }

            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt");
            lines[int.Parse(id)] = vv.Id + ";" + vv.Ime + ";" + vv.Prezime + ";" + vv.KorisnickoIme + ";" + vv.Lozinka + ";" + vv.JMBG + ";" + vv.KontaktTelefon + ";" + vv.Pol + ";" + vv.Email + ";" + vv.Lokacija.X + ";" + vv.Lokacija.Y + ";" + vv.Lokacija.Adresa.UlicaBroj + ";" + vv.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Automobil.Broj + ";" + vv.Automobil.Godiste + ";" + vv.Automobil.Registracija + ";" + vv.Automobil.Tip + ";" + vv.Zauzet+";"+vv.Ban;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt", lines);

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;
            return true;
        }
  

        public bool Post([FromBody]Vozac vozac)
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];

            foreach (var v in vozaci.list)
            {
                if (v.Value.KorisnickoIme == vozac.KorisnickoIme)
                    return true;
            }

            foreach (var v in korisnici.list)
            {
                if (v.Value.KorisnickoIme == vozac.KorisnickoIme)
                    return true;
            }

            foreach (var v in dispeceri.list)
            {
                if (v.Value.KorisnickoIme == vozac.KorisnickoIme)
                    return true;
            }

            string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\vozaci.txt";
            StringBuilder sb = new StringBuilder();
            vozac.Id = vozaci.list.Count.ToString();
            vozac.Automobil.Broj = (vozaci.list.Count + 100).ToString();
            sb.Append(vozac.Id + ";" + vozac.Ime + ";" + vozac.Prezime + ";" + vozac.KorisnickoIme + ";" + vozac.Lozinka + ";" + vozac.JMBG + ";" + vozac.KontaktTelefon + ";" + vozac.Pol + ";" + vozac.Email + ";" + vozac.Lokacija.X + ";" + vozac.Lokacija.Y + ";" + vozac.Lokacija.Adresa.UlicaBroj + ";" + vozac.Lokacija.Adresa.NaseljenoMesto + ";" + vozac.Lokacija.Adresa.PozivniBrojMesta + ";" + vozac.Automobil.Broj + ";" + vozac.Automobil.Godiste + ";" + vozac.Automobil.Registracija + ";" + vozac.Automobil.Tip + ";" + vozac.Zauzet+";" + vozac.Ban+ "\n");

            if (!File.Exists(path))
                File.WriteAllText(path, sb.ToString());
            else
                File.AppendAllText(path, sb.ToString());

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;
            return false;
        }

        public List<Vozac> Get() 
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            List<Vozac> lista = new List<Vozac>();

            foreach (var v in vozaci.list)
                lista.Add(v.Value);

            return lista;
        }


        public Vozac Get(string id) 
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            return vozaci.list[id];
        }
    }
}
