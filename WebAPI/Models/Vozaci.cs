using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebAPI.Models
{
    public class Vozaci
    {
        public Dictionary<string, Vozac> list { get; set; }

        public Vozaci(string path)
        {

            path = HostingEnvironment.MapPath(path);
            list = new Dictionary<string, Vozac>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Vozac p = new Vozac(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8],
                    double.Parse(tokens[9]),double.Parse(tokens[10]),tokens[11], tokens[12], tokens[13], tokens[14], int.Parse(tokens[15]), 
                    tokens[16], tokens[17],int.Parse(tokens[18]), int.Parse(tokens[19]));
                p.Id = list.Count.ToString();
                list.Add(p.Id, p);
            }
            sr.Close();
            stream.Close();
        }



    }
}