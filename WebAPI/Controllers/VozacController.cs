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
using WebAPI.Models.Enums;

namespace WebAPI.Controllers
{
    public class VozacController : ApiController
    {
       
        public bool Put(string id, [FromBody]Vozac vozac)//menjam samo vozaca
        {

            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];//sve voznje

            //Validacija
            if (vozaci.list == null)
                vozaci.list = new Dictionary<string, Vozac>();

            if (!(int.Parse(id) >= 0 && int.Parse(id) < vozaci.list.Count))
                return false;

            //Nije u validnom stanju 
            if (vozac.Ban != Models.Enums.Enumss.Banovan.IGNORE && vozac.Ban != Models.Enums.Enumss.Banovan.DA && vozac.Ban != Models.Enums.Enumss.Banovan.NE)
                return false;

            if (vozac.Zauzet != Models.Enums.Enumss.Zauzet.IGNORE && vozac.Zauzet != Models.Enums.Enumss.Zauzet.DA && vozac.Zauzet != Models.Enums.Enumss.Zauzet.NE)
                return false;


            Vozac vv = vozaci.list[id];
            if (vozac.Ban != Enumss.Banovan.IGNORE)
            {
                vv.Ban = vozac.Ban;
            }

            if (vozac.Zauzet != Enumss.Zauzet.IGNORE)
            {
                vv.Zauzet = vozac.Zauzet;
            }

            string path = "~/App_Data/vozaci.txt";
            path = HostingEnvironment.MapPath(path);

            var lines = File.ReadAllLines(path);
            lines[int.Parse(id)] = vv.Id + ";" + vv.Ime + ";" + vv.Prezime + ";" + vv.KorisnickoIme + ";" + vv.Lozinka + ";" + vv.JMBG + ";" + vv.KontaktTelefon + ";" + vv.Pol + ";" + vv.Email + ";" + vv.Lokacija.X + ";" + vv.Lokacija.Y + ";" + vv.Lokacija.Adresa.UlicaBroj + ";" + vv.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Automobil.Broj + ";" + vv.Automobil.Godiste + ";" + vv.Automobil.Registracija + ";" + vv.Automobil.Tip + ";" + vv.Zauzet+";"+vv.Ban;
            File.WriteAllLines(path, lines);

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;
            return true;
        }
  

        public bool Post([FromBody]Vozac vozac)
        {
            //validacija svakog polja ako je null ili equals "" // u novu metodu

            if (vozac == null)
                return false;
            if (vozac.Lokacija == null)
                return false;
            if (vozac.Automobil == null)
                return false;

            if (String.IsNullOrEmpty(vozac.KorisnickoIme) || String.IsNullOrEmpty(vozac.Lozinka) || String.IsNullOrEmpty(vozac.Ime) || String.IsNullOrEmpty(vozac.Prezime) || String.IsNullOrEmpty((vozac.Pol).ToString()) || String.IsNullOrEmpty(vozac.JMBG) || String.IsNullOrEmpty(vozac.KontaktTelefon) || String.IsNullOrEmpty(vozac.Email))
                return false;

            Regex jmbgReg = new Regex("[0-9]{13}");
            Regex telefonReg = new Regex("[0-9]{6,14}");
            Regex emailReg = new Regex(@"[a-z0-9._% +-]+@[a-z0-9.-]+\.[a-z]{2,3}$");
            Regex korImeReg = new Regex("[0-9a-zA-Z]{4,}");
            Regex lozinkaReg = new Regex("[0-9a-zA-Z]{8,}");

            if (!jmbgReg.IsMatch(vozac.JMBG) || !telefonReg.IsMatch(vozac.KontaktTelefon) || !emailReg.IsMatch(vozac.Email) || !korImeReg.IsMatch(vozac.KorisnickoIme) || !lozinkaReg.IsMatch(vozac.Lozinka))
                return false;

            if (vozac.Automobil.Tip != Enumss.TipAuta.Kombi && vozac.Automobil.Tip != Enumss.TipAuta.Putnicki)
                return false;


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

            string path = "~/App_Data/vozaci.txt";
            path = HostingEnvironment.MapPath(path);

            StringBuilder sb = new StringBuilder();
            vozac.Id = vozaci.list.Count.ToString();//generise se automatski
            vozac.Automobil.Broj = (vozaci.list.Count + 100).ToString();//generise se automatski
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

            //Validacija
            if (vozaci.list == null)
                vozaci.list = new Dictionary<string, Vozac>();

            List<Vozac> lista = new List<Vozac>();
            foreach (var v in vozaci.list)
                lista.Add(v.Value);

            return lista;
        }


        public Vozac Get(string id) 
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            if (int.Parse(id)>= 0 && int.Parse(id) < vozaci.list.Count)
            {
                return vozaci.list[id];
            }
            else
            {
                return null;
            }
        }
    }
}
