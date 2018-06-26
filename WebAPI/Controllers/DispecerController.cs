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
    public class DispecerController : ApiController
    {

        public Dispecer Get(string id)
        {
            Dispeceri dispeceri = (Dispeceri)HttpContext.Current.Application["dispeceri"];
            //Validacija
            if (int.Parse(id) >= 0 && int.Parse(id) < dispeceri.list.Count)
            {
                return dispeceri.list[id];
            }
            else
            {
                return null;
            }
        }

    }
}
