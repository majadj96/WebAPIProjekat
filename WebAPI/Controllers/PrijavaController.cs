﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PrijavaController : ApiController
    {

        public string Post([FromBody]Korisnik korisnik)
        {
           /* var session = HttpContext.Current.Session;


            if (session != null)
                session["korisnik"] = k;*/


            Korisnici korisnici = (Korisnici)HttpContext.Current.Application["korisnici"];

            foreach(var k in korisnici.list)
            {
                if(k.Value.KorisnickoIme==korisnik.KorisnickoIme && k.Value.Lozinka == korisnik.Lozinka)
                {

                    return "Uspesno";
                }
            }

            return "Neuspesna prijava";


        }


    }
}