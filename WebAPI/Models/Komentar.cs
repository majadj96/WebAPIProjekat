using static WebAPI.Models.Enums.Enumss;

namespace WebAPI.Models
{
    public class Komentar
    {

        public string Opis { get; set; }
        public string DatumObjave { get; set; }
        public Korisnik Korisnik { get; set; }
        public Voznja Voznja { get; set; }

        public int Ocena { get; set; }


    }
}