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
    public class KomentarController : ApiController
    {


        public bool Post([FromBody]Komentar komentar) //Za postavljanje komentara
        {

            //opis unosis i ocenu

            Komentari komentari = (Komentari)HttpContext.Current.Application["komentari"];

            komentar.Id = komentari.list.Count().ToString();
            komentar.DatumObjave = DateTime.Now.ToString();

            string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\komentari.txt";
            StringBuilder sb = new StringBuilder();

            sb.Append(komentar.Id+";"+komentar.Opis + ";" + komentar.DatumObjave +";"+ komentar.idKorisnik + ";" + komentar.idVoznja + ";" + komentar.Ocena + "\n"); 

            if (!File.Exists(path))
                File.WriteAllText(path, sb.ToString());
            else
                File.AppendAllText(path, sb.ToString());

            komentari = new Komentari("~/App_Data/komentari.txt");
            HttpContext.Current.Application["komentari"] = komentari;


            //hocu da popunim voznju sa komentarom

            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            string id = komentar.idVoznja;

            Voznja voki = new Voznja();
            foreach (var v in voznje.list)
            {
                if (v.Value.Id == id)
                {
                    voki = v.Value;
                    break;
                }
            }

            voki.Komentar = komentar;


            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt");
            lines[int.Parse(id)] = voki.Id + ";" + voki.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voki.Lokacija.X + ";" + voki.Lokacija.Y + ";" + voki.Lokacija.Adresa.UlicaBroj + ";" + voki.Lokacija.Adresa.NaseljenoMesto + ";" + voki.Lokacija.Adresa.PozivniBrojMesta + ";" + voki.Automobil + ";" + voki.idKorisnik + ";" + voki.Odrediste.X + ";" + voki.Odrediste.Y + ";" + voki.Odrediste.Adresa.UlicaBroj + ";" + voki.Odrediste.Adresa.NaseljenoMesto + ";" + voki.Odrediste.Adresa.PozivniBrojMesta + ";" + voki.idDispecer + ";" + voki.idVozac + ";" + voki.Iznos + ";" + voki.Komentar.Opis + ";" + voki.Komentar.DatumObjave + ";" + voki.Komentar.idKorisnik + ";" + voki.Komentar.idVoznja + ";" + voki.Komentar.Ocena + ";" + voki.StatusVoznje;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt", lines);

            voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;
            return true;

        }



    }
}
