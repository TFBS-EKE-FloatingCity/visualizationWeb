namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Simulations",
                c => new
                    {
                        SimulationTimeID = c.Int(nullable: false, identity: true),
                        SimTypeID = c.Int(nullable: false),
                        RealStartTime = c.DateTime(nullable: false),
                        SimFactor = c.Double(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SimulationTimeID);
            
            AddColumn("dbo.SensorDatas", "SimulationTimeID", c => c.Int(nullable: false));
            AddColumn("dbo.SensorDatas", "MyProperty", c => c.Int(nullable: false));
            CreateIndex("dbo.SensorDatas", "SimulationTimeID");
            AddForeignKey("dbo.SensorDatas", "SimulationTimeID", "dbo.Simulations", "SimulationTimeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorDatas", "SimulationTimeID", "dbo.Simulations");
            DropIndex("dbo.SensorDatas", new[] { "SimulationTimeID" });
            DropColumn("dbo.SensorDatas", "MyProperty");
            DropColumn("dbo.SensorDatas", "SimulationTimeID");
            DropTable("dbo.Simulations");
        }
    }
}
