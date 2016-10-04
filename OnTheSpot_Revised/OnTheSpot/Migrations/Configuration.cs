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
                 //Australian date format
                 new Order { OrderID = 1,  Username = "Eeny", PickupAddress = "Cairns", DeliveryAddress = "20 Fairfield Rd, Annerley", OrderSubmitted = DateTime.Parse("21/09/2016"), Completed = false},
                 new Order { OrderID = 2,  Username = "Eeny", PickupAddress = "Here", DeliveryAddress = "There", OrderSubmitted = DateTime.Parse("20/09/2016"), Completed = false},
                 new Order { OrderID = 3,  Username = "Moe", PickupAddress = "Underhill", DeliveryAddress = "Mordor", OrderSubmitted = DateTime.Parse("02/07/2016"), Completed = true},
                 new Order { OrderID = 4,  Username = "Miny", PickupAddress = "Somewhere", DeliveryAddress = "Anywhere", OrderSubmitted = DateTime.Parse("05/08/2016"), Completed = true},
                 new Order { OrderID = 5,  Username = "Meeny", PickupAddress = "Brisbane", DeliveryAddress = "Gold Coast", OrderSubmitted = DateTime.Parse("22/09/2016"), Completed = false}

            };
            orders.ForEach(s => context.Orders.AddOrUpdate(p => p.OrderID, s));
            context.SaveChanges();

            // Create new packages
            var packages = new List<Package>
            {
                //Australian date format
                new Package {PackageID = 1, OrderID = 1, Status = Status.AtWarehouse, Weight = 15, Collected = DateTime.Parse("21/09/2016"), Delivered = null, Priority = Priority.High, AssignedCourier = "ABug"},
                new Package {PackageID = 2, OrderID = 1, Status = Status.AtWarehouse, Weight = 2, Collected = DateTime.Parse("21/09/2016"), Delivered = null, Priority = Priority.High, AssignedCourier = "ABug"},
                new Package {PackageID = 3, OrderID = 2, Status = Status.ReadyForPickup, Weight = 50, Collected = DateTime.Parse("20/09/2016"), Delivered = null, Priority = Priority.Standard, AssignedCourier = "ALug"},
                new Package {PackageID = 4, OrderID = 3, Status = Status.Delivered, Weight = 1, Collected = DateTime.Parse("02/07/2016"), Delivered = DateTime.Parse("03/07/2016"), Priority = Priority.Overnight, AssignedCourier = "ALug"},
                new Package {PackageID = 5, OrderID = 4, Status = Status.Delivered, Weight = 7, Collected = DateTime.Parse("05/08/2016"), Delivered = DateTime.Parse("09/05/2016"), Priority = Priority.Standard, AssignedCourier = "ABug"},
                new Package {PackageID = 6, OrderID = 5, Status = Status.InTransit, Weight = 4, Collected = DateTime.Parse("22/09/2016"), Delivered = null, Priority = Priority.Standard, AssignedCourier = "ABug"}

            };
            packages.ForEach(s => context.Packages.AddOrUpdate(p => p.PackageID, s));
            context.SaveChanges();

        }
    }
}