using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetterServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PetterServiceContext() : base("name=PetterServiceContext")
        {
        }

        public System.Data.Entity.DbSet<PetterService.Models.Pension> Pensions { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BeautyShop> BeautyShops { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PensionHoliday> PensionHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PensionService> PensionServices { get; set; }
    }
}
