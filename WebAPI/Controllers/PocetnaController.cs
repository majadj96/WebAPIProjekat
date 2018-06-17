using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PocetnaController : ApiController //mozes i ti kokrisnik da budes
    {
        public Korisnik Post([FromBody]Korisnik korisnik)
        {
            
            Korisnik k=null;
            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];


            foreach (var kk in korisnici.list)
            {
                if(kk.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Musterija;
                    return k;
                }
            }

            foreach (var kk in dispeceri.list)
            {
                if (kk.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Dispecer ;

                    return k;
                }
            }

            foreach (var kk in vozaci.list)
            {
                if (kk.Value.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    k = kk.Value;
                    k.Uloga = Models.Enums.Enumss.Uloga.Vozac;

                    return k;
                }
            }


            return k;
        }


    }
}
