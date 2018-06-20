﻿using System;
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

        public Voznja Get(int id)
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            Voznja v = voznje.list[id.ToString()];

            return v;
        }

        public bool Put(string id, [FromBody]Voznja voznja)
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            Voznja voki = voznje.list[id];


            if (voznja.Automobil != 0)
                voki.Automobil = voznja.Automobil;

            if (voznja.idDispecer != null)
                voki.idDispecer = voznja.idDispecer;

            if (voznja.idKorisnik != null)
                voki.idKorisnik = voznja.idKorisnik;

            if (voznja.idVozac != null)
                voki.idVozac = voznja.idVozac;

            if (voznja.Iznos != 0)
                voki.Iznos = voznja.Iznos;

            if (voznja.Komentar != null)
            {
                if (voki.Komentar.Opis != " ")
                {
                    return false;
                }
                voznja.Komentar.DatumObjave = DateTime.Now.ToString();
                voki.Komentar = voznja.Komentar;
            }

            if (voznja.Lokacija != null)
                voki.Lokacija = voznja.Lokacija;

            if (voznja.Ocena != 0)
                voki.Ocena = voznja.Ocena;

            if (voznja.Odrediste != null)
                voki.Odrediste = voznja.Odrediste;

            if (voznja.StatusVoznje != 0)
            {
                if (voznja.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Prihvacena)
                {
                    if (voki.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Prihvacena || voki.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Neuspesna)
                        return false;
                }

                else if (voznja.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Uspesna)
                {
                    if (voki.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Uspesna || voki.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Neuspesna)
                        return false;
                }
                else if (voznja.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Neuspesna)
                {
                    if (voki.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Uspesna || voki.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Neuspesna)
                        return false;
                }else if (voznja.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Otkazana)
                {
                    if (voki.idDispecer != " ")
                    {
                        return false;
                    }
                }else if (voznja.StatusVoznje == Models.Enums.Enumss.StatusVoznje.Obradjena)
                {
                    return false;
                }
                
                voki.StatusVoznje = voznja.StatusVoznje;
            }


            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt");
            lines[int.Parse(id)] = voki.Id + ";" + voki.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voki.Lokacija.X + ";" + voki.Lokacija.Y + ";" + voki.Lokacija.Adresa.UlicaBroj + ";" + voki.Lokacija.Adresa.NaseljenoMesto + ";" + voki.Lokacija.Adresa.PozivniBrojMesta + ";" + voki.Automobil + ";" + voki.idKorisnik + ";" + voki.Odrediste.X + ";" + voki.Odrediste.Y + ";" + voki.Odrediste.Adresa.UlicaBroj + ";" + voki.Odrediste.Adresa.NaseljenoMesto + ";" + voki.Odrediste.Adresa.PozivniBrojMesta + ";" + voki.idDispecer + ";" + voki.idVozac + ";" + voki.Iznos + ";" + voki.Komentar.Opis + ";" + voki.Komentar.DatumObjave + ";" + voki.Komentar.idKorisnik + ";" + voki.Komentar.idVoznja + ";" + voki.Komentar.Ocena + ";" + voki.StatusVoznje;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt", lines);

            voznje = new Voznje("~/App_Data/voznje.txt");
            HttpContext.Current.Application["voznje"] = voznje;
            return true;
        }

        public List<Voznja> Get()
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];
            List<Voznja> kreiraneVoznje = new List<Voznja>();
            foreach (var v in voznje.list)
                kreiraneVoznje.Add(v.Value);

            return kreiraneVoznje;
        }


        public bool Post([FromBody]Voznja voznja)
        {
            Voznje voznje = (Voznje)HttpContext.Current.Application["voznje"];

            string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\voznje.txt";
            StringBuilder sb = new StringBuilder();
            voznja.Id = voznje.list.Count.ToString();
            voznja.DatumVreme = DateTime.Now;

            sb.Append(voznja.Id + ";" + voznja.DatumVreme.ToString("MM/dd/yyyy HH:mm") + ";" + voznja.Lokacija.X + ";" + voznja.Lokacija.Y + ";" + voznja.Lokacija.Adresa.UlicaBroj + ";" + voznja.Lokacija.Adresa.NaseljenoMesto + ";" + voznja.Lokacija.Adresa.PozivniBrojMesta + ";" + voznja.Automobil + ";" + voznja.idKorisnik + ";0;0; ; ; ;" + voznja.idDispecer + ";" + voznja.idVozac+";0; ; ; ; ; ;" + voznja.StatusVoznje + "\n");

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
   
