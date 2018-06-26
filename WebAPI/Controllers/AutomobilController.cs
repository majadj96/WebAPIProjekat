using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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

            if (id >= 0 && id < vozaci.list.Count)
            {
                return vozaci.list[id.ToString()].Automobil;
            }
            else
            {
                return null;
            }
        }

        public bool Put(int id, [FromBody]Automobil automobil) //Promena automobila - izmena
        {
            //Validacija
            if (automobil == null)
                return false;

            if (String.IsNullOrEmpty(automobil.Broj) || String.IsNullOrEmpty(automobil.Registracija))
                return false;

            //Proveri 
            if (automobil.Tip != Models.Enums.Enumss.TipAuta.Kombi && automobil.Tip != Models.Enums.Enumss.TipAuta.Putnicki && automobil.Tip != Models.Enums.Enumss.TipAuta.Svejedno)
                return false;

            Regex RegReg = new Regex("[A-Z]{2}[0-9]{3}[A-Z]{2}");
            if (!RegReg.IsMatch(automobil.Registracija))
                return false;

            if (automobil.Godiste > 2018 || automobil.Godiste < 2011)
                return false;
            

            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            if (vozaci.list == null)
                vozaci.list = new Dictionary<string, Vozac>();

            if (!(id >= 0 && id < vozaci.list.Count))
                return false;
            
          

            Vozac vv = vozaci.list[id.ToString()];
            vv.Automobil.Broj = automobil.Broj;
            vv.Automobil.Registracija = automobil.Registracija;
            vv.Automobil.Tip = automobil.Tip;
            vv.Automobil.Godiste = automobil.Godiste;

            foreach(var v in vozaci.list)
            {
                if (v.Value.Automobil.Broj == vv.Automobil.Broj) //Provera jedinstvenosti
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
