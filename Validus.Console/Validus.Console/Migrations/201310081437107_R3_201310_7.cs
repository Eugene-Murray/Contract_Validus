namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3_201310_7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Submissions", "Description", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Submissions", "Description", c => c.String());
        }
    }
}
