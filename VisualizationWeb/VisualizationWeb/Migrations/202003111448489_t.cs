namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Simulations",
                c => new
                    {
                        SimTypeID = c.Int(nullable: false, identity: true),
                        RealStartTime = c.DateTime(nullable: false),
                        SimFactor = c.Double(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SimTypeID);
            
            AddColumn("dbo.SensorDatas", "Simulation_SimTypeID", c => c.Int());
            CreateIndex("dbo.SensorDatas", "Simulation_SimTypeID");
            AddForeignKey("dbo.SensorDatas", "Simulation_SimTypeID", "dbo.Simulations", "SimTypeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorDatas", "Simulation_SimTypeID", "dbo.Simulations");
            DropIndex("dbo.SensorDatas", new[] { "Simulation_SimTypeID" });
            DropColumn("dbo.SensorDatas", "Simulation_SimTypeID");
            DropTable("dbo.Simulations");
        }
    }
}
