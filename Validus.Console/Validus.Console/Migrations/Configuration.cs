namespace Validus.Console.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Validus.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Validus.Console.Data.ConsoleRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Validus.Console.Data.ConsoleRepository context)
        {            
            Validus.Console.Data.DbInitializer.SeedHelper.SeedData(context);
        }

      
    }
}
