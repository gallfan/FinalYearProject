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
    
    public partial class mechanic
    {
        public mechanic()
        {
            this.notifications = new HashSet<notification>();
            this.jobs = new HashSet<job>();
        }
    
        public int MechanicID { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public sbyte SkillLevel { get; set; }
        public bool Available { get; set; }
        public string Username { get; set; }
        public System.DateTime RowVersion { get; set; }
    
        public virtual ICollection<notification> notifications { get; set; }
        public virtual ICollection<job> jobs { get; set; }
    }
}