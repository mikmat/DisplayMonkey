//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DisplayMonkey.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Frame
    {
        public Frame()
        {
            this.Locations = new HashSet<Location>();
            this.Schedules = new HashSet<Schedule>();
            this.Levels = new HashSet<Level>();
        }
    
        public int FrameId { get; set; }
        public int PanelId { get; set; }
        public int Duration { get; set; }
        public Nullable<System.DateTime> BeginsOn { get; set; }
        public Nullable<System.DateTime> EndsOn { get; set; }
        public Nullable<int> Sort { get; set; }
        public System.DateTime DateCreated { get; protected set; }
        public byte[] Version { get; protected set; }
        public int TemplateId { get; set; }
        public int CacheInterval { get; set; }
        public int ExpirationStatus { get; set; }
    
        public virtual Panel Panel { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual Template Template { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Level> Levels { get; set; }
    }
}
