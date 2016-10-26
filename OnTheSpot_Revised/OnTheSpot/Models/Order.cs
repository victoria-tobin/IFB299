using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace OnTheSpot.Models
{

    //database fields for an order
    public class Order
    {
       
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Please provide username", AllowEmptyStrings = false)]
        public string Username { get; set; }

        public int UserId 
        {
            get { return WebSecurity.GetUserId(Username); }
        }

        public DateTime OrderSubmitted { get; set; }

       // [Required(ErrorMessage = "Please provide a pickup address", AllowEmptyStrings=false)]
        [Display(Name = "Pickup Address")]
        public string PickupAddress { get; set; }

        // [Required(ErrorMessage = "Please provide a delivery address", AllowEmptyStrings=false)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        public bool Pickup { get; set; }
        public bool Completed { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual List<Package> Packages { get; set; }

    }
}