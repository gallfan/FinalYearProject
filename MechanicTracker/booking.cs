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
    
    public partial class booking
    {
        public booking()
        {
            this.jobs = new HashSet<job>();
        }
    
        public int BookingID { get; set; }
        public int CarID { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public string CustomerEmail { get; set; }
        public System.DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public System.DateTime RowVersion { get; set; }
    
        public virtual ICollection<job> jobs { get; set; }
        public virtual car car { get; set; }
    }
}
