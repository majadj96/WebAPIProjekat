using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.Enums
{
    public class Enumss
    {

        public enum Uloga { Musterija, Dispecer, Vozac };
        public enum Pol { Zenski, Muski };

        public enum TipAuta { Svejedno, Putnicki , Kombi };

        public enum StatusVoznje
        {
            Kreirana, // Na cekanju -- INICIRANO
            Formirana,
            Obradjena,
            Prihvacena,
            Otkazana,
            Neuspesna,
            Uspesna,
            Utoku,
        }

    }
}