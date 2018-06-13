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
    public class RegistracijaController : ApiController
    {
        List<Korisnik> korisnici;


        public List<Korisnik> Get()
        {
            if (korisnici == null)
            {
                korisnici = new List<Korisnik>();
                Korisnik k = new Korisnik();
                k.KorisnickoIme = "maja";
                k.Lozinka = "maja";
                korisnici.Add(k);
            }
            return korisnici;
        }

        public bool Post([FromBody]Korisnik korisnik)
        {

            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];


            bool postoji = false;

            foreach (var k in dispeceri.list)
            {
                if (k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    postoji = true;
                    break;
                }


            }


            foreach(var k in korisnici.list)
            {
                if(k.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    postoji = true;
                    break;
                }


            }

      
            if (!postoji)
            {
                string path = @"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt";
                StringBuilder sb = new StringBuilder();
                korisnik.Id = korisnici.list.Count.ToString();
                sb.Append(korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email+"\n");

                if (!File.Exists(path))
                    File.WriteAllText(path, sb.ToString());
                else
                    File.AppendAllText(path, sb.ToString());

                korisnici = new Korisnici("~/App_Data/korisnici.txt");
                HttpContext.Current.Application["korisnici"] = korisnici;
                return true;
            }
            else
            {
                return false;
                //Postoji korisnik
            }
        }





    }


}

