namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3_201310_8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamUnderwriterSettings",
                c => new
                    {
                        TeamId = c.Int(nullable: false),
                        UnderwriterCode = c.String(nullable: false, maxLength: 10),
                        Signature = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.TeamId, t.UnderwriterCode })
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Underwriters", t => t.UnderwriterCode, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UnderwriterCode);
            
            AddColumn("dbo.Quotes", "Description", c => c.String(maxLength: 256));
            AddColumn("dbo.Users", "NonLondonBroker", c => c.String());
            DropColumn("dbo.Submissions", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Submissions", "Description", c => c.String(maxLength: 35));
            DropIndex("dbo.TeamUnderwriterSettings", new[] { "UnderwriterCode" });
            DropIndex("dbo.TeamUnderwriterSettings", new[] { "TeamId" });
            DropForeignKey("dbo.TeamUnderwriterSettings", "UnderwriterCode", "dbo.Underwriters");
            DropForeignKey("dbo.TeamUnderwriterSettings", "TeamId", "dbo.Teams");
            DropColumn("dbo.Users", "NonLondonBroker");
            DropColumn("dbo.Quotes", "Description");
            DropTable("dbo.TeamUnderwriterSettings");
        }
    }
}
