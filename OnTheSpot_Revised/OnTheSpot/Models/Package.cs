using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Priority? Priority { get; set; }

        [Required]
        [Display(Name = "Weight (kg)")]
        public float Weight { get; set; }
        [Required]
        [Display(Name = "Height (cm)")]
        public float Height { get; set; }
        [Required]
        [Display(Name = "Width (cm)")]
        public float Width { get; set; }
        [Required]
        [Display(Name = "Length (cm)")]
        public float Length { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Collected { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Delivered { get; set; }

        public string AssignedCourier { get; set; }
        
        public virtual Order Order { get; set; }
        public virtual UserProfile UserProfile { get; set; }

    }
}