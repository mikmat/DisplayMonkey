using DisplayMonkey.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace DisplayMonkey.Models
{
    public class Schedule
    {
        [DisplayName("ID")]
        public int ScheduleId { get; set; }

        [
            DisplayName("Namn"),
            Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "NameRequired"),
            StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "MaxLengthExceeded")
        ]
        public string ScheduleName { get; set; }
        [
            DisplayName("Starttid"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Starttid krävs")
        ]

        public TimeSpan StartTime { get;  set; }
        [DisplayName("Sluttid")]
        public TimeSpan EndTime { get;  set; }
        [DisplayName("Måndag")]
        public bool Mon { get;  set; }
        [DisplayName("Tisdag")]
        public bool Tue { get;  set; }
        [DisplayName("Onsdag")]
        public bool Wed { get;  set; }
        [DisplayName("Torsdag")]
        public bool Thu { get;  set; }
        [DisplayName("Fredag")]
        public bool Fri { get;  set; }
        [DisplayName("Lördag")]
        public bool Sat { get;  set; }
        [DisplayName("Söndag")]
        public bool Sun { get;  set; }
    }
}