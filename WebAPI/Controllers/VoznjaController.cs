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
    public class VoznjaController : ApiController
    {

        public List<Voznja> Get(string id)//ovo je username
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];

            string korisnickoIme = "";
            foreach (var k in korisnici.list)
            {
                if (k.Value.KorisnickoIme == id)
                {
                    korisnickoIme = k.Key;//to je id
                    break;
                }
            }

            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            List<Voznja> listaKorisnikovihVoznji = new List<Voznja>();
            foreach(var v in voznje.list)
            {
                if (v.Value.idKorisnik == korisnickoIme)
                {
                    listaKorisnikovihVoznji.Add(v.Value);
                }
            }

            return listaKorisnikovihVoznji;
        }

        public bool Post([FromBody]Voznja voznja)
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];

            string korisnickoIme = voznja.idKorisnik;
            foreach(var k in korisnici.list)
            {
                if (k.Value.KorisnickoIme == korisnickoIme)
                {
                    voznja.idKorisnik = k.Key;
                    break;
                }
            }


            string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt";
            StringBuilder sb = new StringBuilder();
            voznja.Id = voznje.list.Count.ToString();
            voznja.DatumVreme = DateTime.Now;
            voznja.StatusVoznje = Models.Enums.Enumss.StatusVoznje.Kreirana;

            sb.Append(voznja.Id + ";" + voznja.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voznja.Lokacija.X + ";" + voznja.Lokacija.Y + ";" + voznja.Lokacija.Adresa.UlicaBroj + ";" + voznja.Lokacija.Adresa.NaseljenoMesto + ";" + voznja.Lokacija.Adresa.PozivniBrojMesta + ";" + voznja.Automobil + ";" + voznja.idKorisnik + ";0;0; ; ; ; ; ;0; ; ; ; ; ;" + voznja.StatusVoznje + "\n");

            if (!File.Exists(path))
                File.WriteAllText(path, sb.ToString());
            else
                File.AppendAllText(path, sb.ToString());

            voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;
            return true;

        }


    }
    }
