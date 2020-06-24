using System;
using System.ComponentModel;


namespace DisplayMonkey.Models
{
    public class Raspberry
    {
        [DisplayName("MAC")]
        public string mac { get; set; }
        [DisplayName("HostName")]
        public string hostname { get; set; }
        [DisplayName("IP")]
        public string ip { get; set; }
        [DisplayName("Inrapporterad")]
        public DateTime updated { get; set; }
        [DisplayName("Temp")]
        public decimal temperature { get; set; }
        [DisplayName("Installerad")]
        public DateTime firstseen { get; set; }
        [DisplayName("Ignorera")]
        public Int32 disabled { get; set; }
        [DisplayName("Ombootad")]
        public DateTime rebooted { get; set; }
    }

}