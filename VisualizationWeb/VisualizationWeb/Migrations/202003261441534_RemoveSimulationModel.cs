namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSimulationModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SensorDatas", "Simulation_SimulationID", "dbo.Simulations");
            DropForeignKey("dbo.SimDatas", "SimulationID", "dbo.Simulations");
            DropIndex("dbo.SensorDatas", new[] { "Simulation_SimulationID" });
            DropIndex("dbo.SimDatas", new[] { "SimulationID" });
            DropColumn("dbo.SensorDatas", "Simulation_SimulationID");
            DropColumn("dbo.SimDatas", "SimulationID");
            DropTable("dbo.Simulations");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.SimDatas", "SimulationID", c => c.Int(nullable: false));
            AddColumn("dbo.SensorDatas", "Simulation_SimulationID", c => c.Int());
            CreateIndex("dbo.SimDatas", "SimulationID");
            CreateIndex("dbo.SensorDatas", "Simulation_SimulationID");
            AddForeignKey("dbo.SimDatas", "SimulationID", "dbo.Simulations", "SimulationID", cascadeDelete: true);
            AddForeignKey("dbo.SensorDatas", "Simulation_SimulationID", "dbo.Simulations", "SimulationID");
        }
    }
}
