using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpCli.Models
{
    public class Antivirus
    {
        public string name { get; set; }
        public string version { get; set; }
        public bool detected { get; set; }
        public string result { get; set; }
        public string update { get; set; }
    }
}