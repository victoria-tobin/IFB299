using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OnTheSpot.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Security;
using WebMatrix.WebData;

namespace OnTheSpot.DAL
{
    public class DatabaseModels : DbContext
    {
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}