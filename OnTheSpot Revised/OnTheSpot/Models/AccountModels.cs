using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using OnTheSpot.DAL;

namespace OnTheSpot.Models
{
    /*
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DatabaseModels")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
 
    */

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string UserRole { get; set; }

        [Required(ErrorMessage = "Please provide username", AllowEmptyStrings = false)]
        public string UserName { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> UserRoles { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> Couriers { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please provide your first name", AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please provide your last name", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
            ErrorMessage = "Please provide valid email id")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a contact number", AllowEmptyStrings = false)]
        public string Phonenumber { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string StreetAddress { get; set; }
        public string Jobtitle { get; set; }
        public Nullable<int> Salary { get; set; }
        public Nullable<int> SumOfWeeklyWorkHours { get; set; }
        public Nullable<int> SumOfWeeklyWorkHoursCompleted { get; set; }

        public RegisterModel()
        {
            UserRole = "Customer";
        }

        public string UserRole { get; set; }
    }

}
