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
    
    public partial class car
    {
        public car()
        {
            this.bookings = new HashSet<booking>();
        }
    
        public int CarID { get; set; }
        public string Registration { get; set; }
        public int ModelID { get; set; }
        public string FuelType { get; set; }
    
        public virtual ICollection<booking> bookings { get; set; }
        public virtual model model { get; set; }
    }
}
