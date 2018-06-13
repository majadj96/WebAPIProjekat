using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebAPI.Models
{
    public class Voznje
    {

        public Dictionary<string, Voznja> list { get; set; }

        public Voznje(string path)
        {

            path = HostingEnvironment.MapPath(path);
            list = new Dictionary<string, Voznja>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Voznja p = new Voznja(tokens[0], tokens[1],double.Parse(tokens[2]), double.Parse(tokens[3]), tokens[4], tokens[5], tokens[6], tokens[7], tokens[8],
                    double.Parse(tokens[9]), double.Parse(tokens[10]), tokens[11], tokens[12], tokens[13], tokens[14],tokens[15],
                    double.Parse(tokens[16]), tokens[17],tokens[18],tokens[19],tokens[20],tokens[21],tokens[22]);
                p.Id = list.Count.ToString();
                list.Add(p.Id, p);
            }
            sr.Close();
            stream.Close();
        }

    }
}