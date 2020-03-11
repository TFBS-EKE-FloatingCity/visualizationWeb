namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsimulationtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Simulations",
                c => new
                    {
                        SimulationID = c.Int(nullable: false, identity: true),
                        SimTypeID = c.Int(nullable: false),
                        RealStartTime = c.DateTime(nullable: false),
                        SimFactor = c.Double(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SimulationID);
            
            AddColumn("dbo.SensorDatas", "SimulationID", c => c.Int(nullable: false));
            AlterColumn("dbo.SimDatas", "SimTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SimTypes", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SimTypes", "EndTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.SensorDatas", "SimulationID");
            AddForeignKey("dbo.SensorDatas", "SimulationID", "dbo.Simulations", "SimulationID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorDatas", "SimulationID", "dbo.Simulations");
            DropIndex("dbo.SensorDatas", new[] { "SimulationID" });
            AlterColumn("dbo.SimTypes", "EndTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.SimTypes", "StartTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.SimDatas", "SimTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.SensorDatas", "SimulationID");
            DropTable("dbo.Simulations");
        }
    }
}
