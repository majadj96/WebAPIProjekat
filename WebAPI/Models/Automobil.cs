using static WebAPI.Models.Enums.Enumss;

namespace WebAPI.Models
{
    public class Automobil
    {
        public Automobil() { }
        public string Broj { get; set; } //ID
        public Vozac Vozac { get; set; }
        public int Godiste { get; set; }
        public string Registracija { get; set; }
        public TipAuta Tip { get; set; }
    }
}