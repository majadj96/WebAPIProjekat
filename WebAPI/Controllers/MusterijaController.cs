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
    public class MusterijaController : ApiController
    {

        public List<Korisnik> Get()
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            List<Korisnik> korisnicici = new List<Korisnik>();
            foreach (var k in korisnici.list)
                korisnicici.Add(k.Value);

            return korisnicici;
        }

        public bool Put(int id, [FromBody]Korisnik value)
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];

            Korisnik korisnik = korisnici.list[id.ToString()];

            korisnik.Ban = value.Ban;

            string path = "~/App_Data/korisnici.txt";
            path = HostingEnvironment.MapPath(path);

            var lines = File.ReadAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt");
            lines[id] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email + ";" + korisnik.Ban;
            File.WriteAllLines(@"C:\Users\john\Desktop\WebAPI\WebAPI\App_Data\korisnici.txt", lines);

            korisnici = new Korisnici("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;

            return true;
        }

    }
    }
