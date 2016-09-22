namespace OnTheSpot.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using OnTheSpot.Models;
    using System.Collections.Generic;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<OnTheSpot.DAL.DatabaseModels>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OnTheSpot.DAL.DatabaseModels context)
        {

            if (!WebMatrix.WebData.WebSecurity.Initialized)
            {
                WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("DatabaseModels",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            // Create roles if they don't exist
            if (!Roles.RoleExists("Admin"))
            {
                Roles.CreateRole("Admin");
            }

            if (!Roles.RoleExists("Courier"))
            {
                Roles.CreateRole("Courier");
            }

            if (!Roles.RoleExists("Customer"))
            {
                Roles.CreateRole("Customer");
            }


            // Create users if they don't exist
            if (!WebSecurity.UserExists("BWiley"))
            {
                WebSecurity.CreateUserAndAccount("BWiley", "admin1", propertyValues: new { UserRole = "Admin" });
                Roles.AddUserToRole("BWiley", "Admin");
            }

            if (!WebSecurity.UserExists("ALug"))
            {
                WebSecurity.CreateUserAndAccount("ALug", "password", propertyValues: new { UserRole = "Courier" });
                Roles.AddUserToRole("ALug", "Courier");
            }

            if (!WebSecurity.UserExists("ABug"))
            {
                WebSecurity.CreateUserAndAccount("ABug", "password", propertyValues: new { UserRole = "Courier" });
                Roles.AddUserToRole("ABug", "Courier");
            }

            if (!WebSecurity.UserExists("Eeny"))
            {
                WebSecurity.CreateUserAndAccount("Eeny", "password", propertyValues: new { UserRole = "Customer" });
                Roles.AddUserToRole("Eeny", "Customer");
            }

            if (!WebSecurity.UserExists("Meeny"))
            {
                WebSecurity.CreateUserAndAccount("Meeny", "password", propertyValues: new { UserRole = "Customer" });
                Roles.AddUserToRole("Meeny", "Customer");
            }

            if (!WebSecurity.UserExists("Miny"))
            {
                WebSecurity.CreateUserAndAccount("Miny", "password", propertyValues: new { UserRole = "Customer" });
                Roles.AddUserToRole("Miny", "Customer");
            }

            if (!WebSecurity.UserExists("Moe"))
            {
                WebSecurity.CreateUserAndAccount("Moe", "password", propertyValues: new { UserRole = "Customer" });
                Roles.AddUserToRole("Moe", "Customer");
            }


            // Create new orders
            var orders = new List<Order>
            {
                 new Order { OrderID = 1,  Username = "Eeny", OrderSubmitted = DateTime.Parse("1/09/2016"), Completed = false, Pickup = false},
                 new Order { OrderID = 2,  Username = "Eeny", OrderSubmitted = DateTime.Parse("31/08/2016"), Completed = true, Pickup = false},
                 new Order { OrderID = 3,  Username = "Moe", OrderSubmitted = DateTime.Parse("1/08/2016"), Completed = false, Pickup = true}
                 //new Order { OrderID = 4,  UserName = "Miny", OrderSubmitted = DateTime.Parse("15/07/2016"), Completed = false},
                 //new Order { OrderID = 5,  UserName = "Meeny", OrderSubmitted = DateTime.Parse("2/07/2016"), Completed = false}
            };
            orders.ForEach(s => context.Orders.AddOrUpdate(p => p.OrderID, s));
            context.SaveChanges();

            // Create new packages
            var packages = new List<Package>
            {
                new Package {PackageID = 1, OrderID = 1, Status = Status.AtWarehouse, PickupAddress = "Location 1", DeliveryAddress = "56 Albert St, Brisbane CBD", Weight = 15, Collected = DateTime.Parse("2/07/2016"), Delivered = null, Priority = Priority.High, AssignedCourier = "ABug"}

            };
            packages.ForEach(s => context.Packages.AddOrUpdate(p => p.PackageID, s));
            context.SaveChanges();

        }
    }
}