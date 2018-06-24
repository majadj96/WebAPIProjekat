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
    public class AutomobilController : ApiController // Za izmenu automobila i dobavljanje istog, kao entitet Automobil ( u Vozacu ) :)
    {
        public Automobil Get(int id)
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];
            return vozaci.list[id.ToString()].Automobil;
        }

        public bool Put(int id, [FromBody]Automobil automobil)
        {
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            Vozac vv = vozaci.list[id.ToString()];
            vv.Automobil.Broj = automobil.Broj;
            vv.Automobil.Registracija = automobil.Registracija;
            vv.Automobil.Tip = automobil.Tip;
            vv.Automobil.Godiste = automobil.Godiste;

            foreach(var v in vozaci.list)
            {
                if (v.Value.Automobil.Broj == vv.Automobil.Broj)
                {
                    if (v.Key != vv.Id)
                    {
                        return false;
                    }
                }
            }

            string path = HostingEnvironment.MapPath("~/App_Data/vozaci.txt");


            var lines = File.ReadAllLines(path);
            lines[id] = vv.Id + ";" + vv.Ime + ";" + vv.Prezime + ";" + vv.KorisnickoIme + ";" + vv.Lozinka + ";" + vv.JMBG + ";" + vv.KontaktTelefon + ";" + vv.Pol + ";" + vv.Email + ";" + vv.Lokacija.X + ";" + vv.Lokacija.Y + ";" + vv.Lokacija.Adresa.UlicaBroj + ";" + vv.Lokacija.Adresa.NaseljenoMesto + ";" + vv.Lokacija.Adresa.PozivniBrojMesta + ";" + vv.Automobil.Broj + ";" + vv.Automobil.Godiste + ";" + vv.Automobil.Registracija + ";" + vv.Automobil.Tip + ";" + vv.Zauzet+";"+vv.Ban;
            File.WriteAllLines(path, lines);

            vozaci = new Vozaci("~/App_Data/vozaci.txt");
            HttpContext.Current.Application["vozaci"] = vozaci;
            return true;


        }

    }
}
