﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MTEntities : DbContext
    {
        public MTEntities()
            : base("name=MTEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<booking> bookings { get; set; }
        public DbSet<car> cars { get; set; }
        public DbSet<job> jobs { get; set; }
        public DbSet<jobscategory> jobscategories { get; set; }
        public DbSet<jobslist> jobslists { get; set; }
        public DbSet<make> makes { get; set; }
        public DbSet<mechanic> mechanics { get; set; }
        public DbSet<model> models { get; set; }
        public DbSet<model_jobslist> model_jobslist { get; set; }
        public DbSet<notification> notifications { get; set; }
        public DbSet<timessheet> timessheets { get; set; }
    }
}
