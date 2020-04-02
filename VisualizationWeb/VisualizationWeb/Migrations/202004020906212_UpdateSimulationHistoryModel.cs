namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSimulationHistoryModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SimulationHistories", "SimType_SimTypeID", "dbo.SimTypes");
            DropIndex("dbo.SimulationHistories", new[] { "SimType_SimTypeID" });
            RenameColumn(table: "dbo.SimulationHistories", name: "SimType_SimTypeID", newName: "SimTypeID");
            AlterColumn("dbo.SimulationHistories", "Canceled", c => c.DateTime());
            AlterColumn("dbo.SimulationHistories", "SimTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.SimulationHistories", "SimTypeID");
            AddForeignKey("dbo.SimulationHistories", "SimTypeID", "dbo.SimTypes", "SimTypeID", cascadeDelete: true);
            DropColumn("dbo.SimulationHistories", "SimulationID");
            DropColumn("dbo.SimulationHistories", "CancelTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SimulationHistories", "CancelTime", c => c.DateTime());
            AddColumn("dbo.SimulationHistories", "SimulationID", c => c.Int(nullable: false));
            DropForeignKey("dbo.SimulationHistories", "SimTypeID", "dbo.SimTypes");
            DropIndex("dbo.SimulationHistories", new[] { "SimTypeID" });
            AlterColumn("dbo.SimulationHistories", "SimTypeID", c => c.Int());
            AlterColumn("dbo.SimulationHistories", "Canceled", c => c.Boolean(nullable: false));
            RenameColumn(table: "dbo.SimulationHistories", name: "SimTypeID", newName: "SimType_SimTypeID");
            CreateIndex("dbo.SimulationHistories", "SimType_SimTypeID");
            AddForeignKey("dbo.SimulationHistories", "SimType_SimTypeID", "dbo.SimTypes", "SimTypeID");
        }
    }
}
