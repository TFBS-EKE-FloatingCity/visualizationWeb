namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingsRework : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Settings", "WindMax", c => c.Int(nullable: false));
            AlterColumn("dbo.Settings", "SunMax", c => c.Int(nullable: false));
            AlterColumn("dbo.Settings", "ConsumptionMax", c => c.Int(nullable: false));
            DropColumn("dbo.Settings", "WindActive");
            DropColumn("dbo.Settings", "SunActive");
            DropColumn("dbo.Settings", "ConsumptionActive");
            DropTable("dbo.SimulationServiceSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SimulationServiceSettings",
                c => new
                    {
                        OptionName = c.String(nullable: false, maxLength: 128),
                        OptionValue = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.OptionName);
            
            AddColumn("dbo.Settings", "ConsumptionActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Settings", "SunActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Settings", "WindActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Settings", "ConsumptionMax", c => c.Double(nullable: false));
            AlterColumn("dbo.Settings", "SunMax", c => c.Double(nullable: false));
            AlterColumn("dbo.Settings", "WindMax", c => c.Double(nullable: false));
        }
    }
}
