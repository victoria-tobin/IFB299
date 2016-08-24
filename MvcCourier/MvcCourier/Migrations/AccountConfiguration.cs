namespace MvcCourier.Migrations
{
    using MvcCourier.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class AccountConfiguration : DbMigrationsConfiguration<MvcCourier.Models.UsersContext>
    {
        public AccountConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        //  This method will be called after migrating to the latest version.
        protected override void Seed(MvcCourier.Models.UsersContext context)
        {

            //  to update the database after change, run following command in Package  Manager
            // Console: update-database -ConfigurationTypeName AccountConfiguration

            if (!WebMatrix.WebData.WebSecurity.Initialized) 
            {
                WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
            if (!Roles.RoleExists("Admin"))
            {
                Roles.CreateRole("Admin");

            }
            if (!WebSecurity.UserExists("BWiley"))
            {
            WebSecurity.CreateUserAndAccount("BWiley", "admin1", propertyValues: new { FirstName = "Bill", LastName = "Wiley", UserRole = "Admin" });
            Roles.AddUserToRole("BWiley", "Admin");
            }

            if (!Roles.RoleExists("Customer"))
            {
                Roles.CreateRole("Customer");
            }

            
            
        }
    }
}
