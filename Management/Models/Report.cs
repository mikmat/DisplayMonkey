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
    
    public partial class Report
    {
        public int FrameId { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public RenderModes Mode { get; set; }
        public int ServerId { get; set; }
    
        public virtual Frame Frame { get; set; }
        public virtual ReportServer ReportServer { get; set; }
    }
}