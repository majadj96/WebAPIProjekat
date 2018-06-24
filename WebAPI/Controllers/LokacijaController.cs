using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class LokacijaController : ApiController // Lokacija kao entitet (u okviru Vozaca), razdvojila sam
    {
        public bool Put(string id, [FromBody]Lokacija lokacija)
        {

            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            Vozac vv = vozaci.list[id];
          
            vv.Lokacija = lokacija;

            string path = "~/App_Data/vozaci.txt";
            path = HostingEnvironment.MapPath(path);

            var lines = File.ReadAllLines(path);
            lines[int.Parse(id)] = vv.Id + ";" + vv.Ime + ";" + vv.Prezime + ";" + vv.KorisnickoIme + ";" + vv.Lozinka + ";" + vv.JMBG + ";" + vv.KontaktTelefon + ";" + vv.Pol + ";" + vv.Email + ";" + vv.Lokacija.X + ";" + vv.Lokacija.Y + ";" + vv.Lokacija.Adresa.UlicaBroj + ";" + vv.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Automobil.Broj + ";" + vv.Automobil.Godiste + ";" + vv.Automobil.Registracija + ";" + vv.Automobil.Tip + ";" + vv.Zauzet+";"+vv.Ban;
            File.WriteAllLines(path, lines);

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;


            return true;

        }
        public Lokacija Get(int id)
        {
            Lokacija lokacija = new Lokacija();
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            lokacija = vozaci.list[id.ToString()].Lokacija;


            return lokacija;
        }

    }
}
