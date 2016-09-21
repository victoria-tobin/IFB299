using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnTheSpot.Models
{

    public enum Priority 
    {
        Standard, High, Overnight
    }

    public enum Status
    {
        ReadyForPickup, AtWarehouse, InTransit, Delivered
    }


    public class Package
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PackageID { get; set; }
        public int OrderID { get; set; }
        public Status? Status { get; set; }
        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public Priority? Priority { get; set; }
        public float Weight { get; set; }
        public DateTime? Collected { get; set; }
        //public DateTime AtWarehouse { get; set; }
        //public DateTime InTransit { get; set; }
        public DateTime? Delivered { get; set; }
        //public bool Signature { get; set; }
        public string AssignedCourier { get; set; }

        public virtual Order Order { get; set; }
        public virtual UserProfile UserProfile { get; set; }



        // Pickup location, 
        // date and time for pick up, 
        // date and time for delivery, 
        // delivery status, 
        // priority, 
        // weight

    }
}