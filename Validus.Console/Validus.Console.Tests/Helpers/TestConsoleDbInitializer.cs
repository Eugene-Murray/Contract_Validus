
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Validus.Console.Data;


namespace Validus.Console.Tests.Helpers
{
    public class TestConsoleDbInitializer : DropCreateDatabaseAlways<ConsoleRepository>
    {
        protected override void Seed(ConsoleRepository context)
        {
            Validus.Console.Data.DbInitializer.SeedHelper.NewSeedData(context);
            context.SaveChanges();
        }
    }
}
