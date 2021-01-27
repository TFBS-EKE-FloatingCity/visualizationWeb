namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClearedUnusedModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SimDatas", "SimTypeID", "dbo.SimTypes");
            DropForeignKey("dbo.SimulationHistories", "SimTypeID", "dbo.SimTypes");
            DropIndex("dbo.SimDatas", new[] { "SimTypeID" });
            DropIndex("dbo.SimulationHistories", new[] { "SimTypeID" });
            DropTable("dbo.SimDatas");
            DropTable("dbo.SimTypes");
            DropTable("dbo.SimulationHistories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SimulationHistories",
                c => new
                    {
                        HistoryID = c.Int(nullable: false, identity: true),
                        RealStartTime = c.DateTime(nullable: false),
                        SimTypeID = c.Int(nullable: false),
                        Canceled = c.DateTime(),
                    })
                .PrimaryKey(t => t.HistoryID);
            
            CreateTable(
                "dbo.SimTypes",
                c => new
                    {
                        SimTypeID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        SimFactor = c.Double(nullable: false),
                        Notes = c.String(maxLength: 200),
                        StartTime = c.DateTime(nullable: false),
                        Interval = c.Time(nullable: false, precision: 7),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SimTypeID);
            
            CreateTable(
                "dbo.SimDatas",
                c => new
                    {
                        SimDataID = c.Int(nullable: false, identity: true),
                        SimTypeID = c.Int(nullable: false),
                        SimTime = c.DateTime(nullable: false),
                        Wind = c.Double(nullable: false),
                        Sun = c.Double(nullable: false),
                        Consumption = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SimDataID);
            
            CreateIndex("dbo.SimulationHistories", "SimTypeID");
            CreateIndex("dbo.SimDatas", "SimTypeID");
            AddForeignKey("dbo.SimulationHistories", "SimTypeID", "dbo.SimTypes", "SimTypeID", cascadeDelete: true);
            AddForeignKey("dbo.SimDatas", "SimTypeID", "dbo.SimTypes", "SimTypeID", cascadeDelete: true);
        }
    }
}
