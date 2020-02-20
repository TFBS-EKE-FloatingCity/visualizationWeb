namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nachbessernSettingsDBSet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingID = c.Int(nullable: false, identity: true),
                        WindMax = c.Double(nullable: false),
                        SunMax = c.Double(nullable: false),
                        ConsumptionMax = c.Double(nullable: false),
                        WindActive = c.Boolean(nullable: false),
                        SunActive = c.Boolean(nullable: false),
                        ConsumptionActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SettingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
