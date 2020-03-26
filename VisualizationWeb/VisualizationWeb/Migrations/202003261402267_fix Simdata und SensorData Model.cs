namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixSimdataundSensorDataModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SensorDatas", "SimulationID", "dbo.Simulations");
            DropIndex("dbo.SensorDatas", new[] { "SimulationID" });
            RenameColumn(table: "dbo.SensorDatas", name: "SimulationID", newName: "Simulation_SimulationID");
            AddColumn("dbo.SensorDatas", "MeasureTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.SimDatas", "RealTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.SimDatas", "SimulationID", c => c.Int(nullable: false));
            AlterColumn("dbo.SensorDatas", "Simulation_SimulationID", c => c.Int());
            CreateIndex("dbo.SensorDatas", "Simulation_SimulationID");
            CreateIndex("dbo.SimDatas", "SimulationID");
            AddForeignKey("dbo.SimDatas", "SimulationID", "dbo.Simulations", "SimulationID", cascadeDelete: true);
            AddForeignKey("dbo.SensorDatas", "Simulation_SimulationID", "dbo.Simulations", "SimulationID");
            DropColumn("dbo.SensorDatas", "RealTime");
            DropColumn("dbo.SensorDatas", "SimulationTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SensorDatas", "SimulationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.SensorDatas", "RealTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.SensorDatas", "Simulation_SimulationID", "dbo.Simulations");
            DropForeignKey("dbo.SimDatas", "SimulationID", "dbo.Simulations");
            DropIndex("dbo.SimDatas", new[] { "SimulationID" });
            DropIndex("dbo.SensorDatas", new[] { "Simulation_SimulationID" });
            AlterColumn("dbo.SensorDatas", "Simulation_SimulationID", c => c.Int(nullable: false));
            DropColumn("dbo.SimDatas", "SimulationID");
            DropColumn("dbo.SimDatas", "RealTime");
            DropColumn("dbo.SensorDatas", "MeasureTime");
            RenameColumn(table: "dbo.SensorDatas", name: "Simulation_SimulationID", newName: "SimulationID");
            CreateIndex("dbo.SensorDatas", "SimulationID");
            AddForeignKey("dbo.SensorDatas", "SimulationID", "dbo.Simulations", "SimulationID", cascadeDelete: true);
        }
    }
}
