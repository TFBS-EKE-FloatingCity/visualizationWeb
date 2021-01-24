namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SimPositions",
                c => new
                    {
                        SimPositionID = c.Int(nullable: false, identity: true),
                        SunValue = c.Int(nullable: false),
                        WindValue = c.Int(nullable: false),
                        EnergyBalanceValue = c.Int(nullable: false),
                        DateRegistered = c.DateTime(nullable: false),
                        SimScenarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SimPositionID)
                .ForeignKey("dbo.SimScenarios", t => t.SimScenarioID, cascadeDelete: true)
                .Index(t => t.SimScenarioID);
            
            CreateTable(
                "dbo.SimScenarios",
                c => new
                    {
                        SimScenarioID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        TimeFactor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 500),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SimScenarioID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SimPositions", "SimScenarioID", "dbo.SimScenarios");
            DropIndex("dbo.SimPositions", new[] { "SimScenarioID" });
            DropTable("dbo.SimScenarios");
            DropTable("dbo.SimPositions");
        }
    }
}
