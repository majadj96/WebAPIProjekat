using System;
using static WebAPI.Models.Enums.Enumss;

namespace WebAPI.Models
{
    public class Komentar
    {

        public string Opis { get; set; }
        public string DatumObjave { get; set; }
        public string idKorisnik { get; set; }
        public string idVoznja { get; set; }

        public string Ocena { get; set; }


    }
}