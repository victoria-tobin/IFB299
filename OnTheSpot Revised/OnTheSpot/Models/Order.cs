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

        public string UserName { get; set; }

        //[ForeignKey("UserProfile")]
        public int UserId //{ get; set; }
        {
            get { return WebSecurity.GetUserId(UserName); }
        }

        public DateTime OrderSubmitted { get; set; }
        public bool Completed { get; set; }

       // public virtual UserProfile UserProfile { get; set; }

    }
}