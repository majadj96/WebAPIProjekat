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

        public List<Korisnik> Get() // Niko ne koristi
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            List<Korisnik> korisnicici = new List<Korisnik>();

            //Validacija u slucaju da aplikacija vrati null
            if (korisnici.list == null)
                korisnici.list = new Dictionary<string, Korisnik>();

            foreach (var k in korisnici.list)
                korisnicici.Add(k.Value);

            return korisnicici;
        }

        public bool Put(int id, [FromBody]Korisnik value)
        {
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];

            //Validacija
            if (korisnici.list == null)
                korisnici.list = new Dictionary<string, Korisnik>();
            
            if (!(id >= 0 && id < korisnici.list.Count))
                return false;

            Korisnik korisnik = korisnici.list[id.ToString()];

            //Nije u validnom stanju - proveri
            if (value.Ban != Models.Enums.Enumss.Banovan.IGNORE && value.Ban != Models.Enums.Enumss.Banovan.DA && value.Ban != Models.Enums.Enumss.Banovan.NE)
                return false;

            if (value.Ban != Models.Enums.Enumss.Banovan.IGNORE)
            korisnik.Ban = value.Ban;

            string path = "~/App_Data/korisnici.txt";
            path = HostingEnvironment.MapPath(path);

            var lines = File.ReadAllLines(path);
            lines[id] = korisnik.Id + ";" + korisnik.Ime + ";" + korisnik.Prezime + ";" + korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.JMBG + ";" + korisnik.KontaktTelefon + ";" + korisnik.Pol + ";" + korisnik.Email + ";" + korisnik.Ban;
            File.WriteAllLines(path, lines);

            korisnici = new Korisnici("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;

            return true;
        }

    }
    }
