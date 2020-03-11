namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class simulationTimeIDtoSimulationIDinSimModel : DbMigration
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
                        StartTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SimulationID);
            
            AddColumn("dbo.SensorDatas", "SimulationID", c => c.Int(nullable: false));
            CreateIndex("dbo.SensorDatas", "SimulationID");
            AddForeignKey("dbo.SensorDatas", "SimulationID", "dbo.Simulations", "SimulationID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorDatas", "SimulationID", "dbo.Simulations");
            DropIndex("dbo.SensorDatas", new[] { "SimulationID" });
            DropColumn("dbo.SensorDatas", "SimulationID");
            DropTable("dbo.Simulations");
        }
    }
}
