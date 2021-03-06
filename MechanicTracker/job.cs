//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MechanicTracker
{
    using System;
    using System.Collections.Generic;
    
    public partial class job
    {
        public job()
        {
            this.timessheets = new HashSet<timessheet>();
            this.notifications = new HashSet<notification>();
        }
    
        public int JobID { get; set; }
        public short JobsListID { get; set; }
        public int BookingID { get; set; }
        public Nullable<short> TimeAllowed { get; set; }
        public Nullable<short> TimeTaken { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> TimeStarted { get; set; }
        public sbyte Difficulty { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<int> MechanicID { get; set; }
        public System.DateTime RowVersion { get; set; }
    
        public virtual booking booking { get; set; }
        public virtual jobslist jobslist { get; set; }
        public virtual ICollection<timessheet> timessheets { get; set; }
        public virtual mechanic mechanic { get; set; }
        public virtual ICollection<notification> notifications { get; set; }
    }
}
