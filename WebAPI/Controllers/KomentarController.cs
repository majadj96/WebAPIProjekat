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
            return true;

        }



    }
}
