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
    public class IzmeniVoznjuController : ApiController
    {


        public void Post([FromBody]Voznja voznja)
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            string id = voznja.Id;
          
            Voznja voki = new Voznja();
          foreach(var v in voznje.list)
            {
                if (v.Value.Id == voznja.Id)
                {
                    voki = v.Value;
                    break;
                }
            }

            voki.DatumVreme = voznja.DatumVreme;
            voki.Automobil = voznja.Automobil;
            voki.Lokacija = voznja.Lokacija;

            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt");
            lines[int.Parse(id)] = voki.Id + ";" + voki.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voki.Lokacija.X + ";" + voki.Lokacija.Y + ";" + voki.Lokacija.Adresa.UlicaBroj + ";" + voki.Lokacija.Adresa.NaseljenoMesto + ";" + voki.Lokacija.Adresa.PozivniBrojMesta + ";" + voki.Automobil + ";" + voki.idKorisnik + ";" + voki.Odrediste.X + ";" + voki.Odrediste.Y + ";" + voki.Odrediste.Adresa.UlicaBroj + ";" + voki.Odrediste.Adresa.NaseljenoMesto + ";" + voki.Odrediste.Adresa.PozivniBrojMesta + ";" + voki.idDispecer + ";" + voki.idVozac + ";" + voki.Iznos + ";" + voki.Komentar.Opis + ";" + voki.Komentar.DatumObjave + ";" + voki.Komentar.idKorisnik + ";" + voki.Komentar.idVoznja + ";" + voki.Komentar.Ocena + ";" + voki.StatusVoznje;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt", lines);

            voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;
        }


    }
}
