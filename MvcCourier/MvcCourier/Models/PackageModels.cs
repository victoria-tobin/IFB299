using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data.Entity;

namespace MvcCourier.Models
{
    public class PackageModels 
    {
        public int ID { get; set; }
        [DisplayName("ID")]
        public string PackageId { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }

    public class PackageDBContext : DbContext
    {
        public DbSet<PackageModels> Packages { get; set; }
    }
}