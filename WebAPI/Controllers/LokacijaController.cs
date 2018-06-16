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
    public class LokacijaController : ApiController
    {

        public Lokacija Get(int id)
        {
            Lokacija lokacija = new Lokacija();
            //preuzecu lokaciju od vozaca:) i vratiti je nazad
            Vozaci vozaci = (Vozaci)HttpContext.Current.Application["vozaci"];

            lokacija = vozaci.list[id.ToString()].Lokacija;


            return lokacija;
        }

    }
}
