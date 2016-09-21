using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace OnTheSpot.Models
{
    public class Order
    {
       
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public string Username { get; set; }

        //[ForeignKey("UserProfile")]
        public int UserId //{ get; set; }
        {
            get { return WebSecurity.GetUserId(Username); }
        }

        public DateTime OrderSubmitted { get; set; }
        public bool Completed { get; set; }
        public bool Pickup { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual IEnumerable<Package> Packages { get; set; }
       // public virtual UserProfile UserProfile { get; set; }

    }
}