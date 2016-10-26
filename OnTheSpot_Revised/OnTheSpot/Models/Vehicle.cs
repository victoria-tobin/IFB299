using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnTheSpot.Models
{
    //enum for allowed vehicle types
    public enum VehicleType
    {
        Car, Van, Truck, Motorbike
    }

    //database fields for a vehicle
    public class Vehicle
    {
        [Key]
        public string Registration { get; set; } //rego number e.g. 123ABC
        public VehicleType? Type { get; set; } 
        public string Owner { get; set; } //name of owner - if owned by employee
        public bool InUse { get; set; } //true for currently being used, false for available
        public string UsedBy { get; set; } //only set if the car is InUse - name of courier currently using the vehicle

        public virtual UserProfile UserProfile { get; set; }

    }
}