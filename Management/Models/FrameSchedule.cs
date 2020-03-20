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
    public partial class FrameSchedule
    {
        [DisplayName("ID")]
        public int ID { get; set; }
        public int Frame2Id { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
    }
}