namespace MvcCourier.Migrations
{
    using MvcCourier.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class PackageConfiguration : DbMigrationsConfiguration<MvcCourier.Models.PackageDBContext>
    {
        public PackageConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MvcCourier.Models.PackageDBContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  update-database -ConfigurationTypeName PackageConfiguration

            context.Packages.AddOrUpdate(
                i => i.PackageId,
                new PackageModels
                {
                    PackageId = "1132",
                    Location = "Warehouse",
                    Status = "In Transit",
                }
             );

        }
    }
}
