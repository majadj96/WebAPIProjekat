using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebAPI.Models
{
    public class Dispeceri
    {
        public Dictionary<string, Dispecer> list { get; set; }

        public Dispeceri(string path)
        {

            path = HostingEnvironment.MapPath(path);
            list = new Dictionary<string, Dispecer>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Dispecer p = new Dispecer(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8]);
                p.Id = list.Count.ToString();
                list.Add(p.Id, p);
            }
            sr.Close();
            stream.Close();
        }




    }
}