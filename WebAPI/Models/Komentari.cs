using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebAPI.Models
{
    public class Komentari
    {

        public List<Komentar> list { get; set; }

        public Komentari(string path)
        {

            path = HostingEnvironment.MapPath(path);
            list = new List<Komentar>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Komentar p = new Komentar(tokens[0], tokens[1], tokens[2], tokens[3],tokens[4],tokens[5]);
                list.Add(p);
            }
            sr.Close();
            stream.Close();
        }


    }
}