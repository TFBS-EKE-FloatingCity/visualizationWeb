namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatetSimHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SimulationHistories",
                c => new
                    {
                        HistoryID = c.Int(nullable: false, identity: true),
                        RealStartTime = c.DateTime(nullable: false),
                        SimulationID = c.Int(nullable: false),
                        Canceled = c.Boolean(nullable: false),
                        CancelTime = c.DateTime(nullable: false),
                        SimType_SimTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.HistoryID)
                .ForeignKey("dbo.SimTypes", t => t.SimType_SimTypeID)
                .Index(t => t.SimType_SimTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SimulationHistories", "SimType_SimTypeID", "dbo.SimTypes");
            DropIndex("dbo.SimulationHistories", new[] { "SimType_SimTypeID" });
            DropTable("dbo.SimulationHistories");
        }
    }
}
