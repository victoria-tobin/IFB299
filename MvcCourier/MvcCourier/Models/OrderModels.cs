using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MvcCourier.Models
{
    public class OrderModels
    {
        [Key]
        public int OrderID { get; set; }
        public int CustomerName { get; set; }
        public bool Delivered { get; set; }
        public int assignedCourier { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Couriers { get; set; }
       
    }

    public class OrdersDBContext : DbContext
    {
        public DbSet<OrderModels> Orders { get; set; }
    }

    // Uninstall-package EntityFramework -force
    // Install-Package EntityFramework -version 5.0.0
    // had entity framework 6.1.3
}