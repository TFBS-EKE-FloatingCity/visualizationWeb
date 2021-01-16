namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityDataHead : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityDataHeads",
                c => new
                    {
                        CityDataHeadID = c.Int(nullable: false, identity: true),
                        SimulationID = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CityDataHeadID);
            
            AddColumn("dbo.CityDatas", "CityDataHeadID", c => c.Int());
            CreateIndex("dbo.CityDatas", "CityDataHeadID");
            AddForeignKey("dbo.CityDatas", "CityDataHeadID", "dbo.CityDataHeads", "CityDataHeadID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CityDatas", "CityDataHeadID", "dbo.CityDataHeads");
            DropIndex("dbo.CityDatas", new[] { "CityDataHeadID" });
            DropColumn("dbo.CityDatas", "CityDataHeadID");
            DropTable("dbo.CityDataHeads");
        }
    }
}
