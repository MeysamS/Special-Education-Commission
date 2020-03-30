using System.Data.Entity.Migrations;
using Comision.Data.Context;

namespace Comision.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ComisionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            // ContextKey = "PPU.Data.Context.PpuContext";
        }

        protected override void Seed(ComisionDbContext context)
        {
            
        }
    }
}