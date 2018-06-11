namespace WebAPI.Models
{
    public class Lokacija
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Adresa Adresa { get; set; }

        public Lokacija() { }
    }
}