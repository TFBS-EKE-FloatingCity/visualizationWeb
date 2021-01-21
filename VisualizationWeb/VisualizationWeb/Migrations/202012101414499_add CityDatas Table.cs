namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCityDatasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityDatas",
                c => new
                    {
                        CityDataID = c.Int(nullable: false, identity: true),
                        USonicInner1 = c.Int(nullable: false),
                        USonicOuter1 = c.Int(nullable: false),
                        Pump1 = c.Int(nullable: false),
                        USonicInner2 = c.Int(nullable: false),
                        USonicOuter2 = c.Int(nullable: false),
                        Pump2 = c.Int(nullable: false),
                        USonicInner3 = c.Int(nullable: false),
                        USonicOuter3 = c.Int(nullable: false),
                        Pump3 = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CityDataID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CityDatas");
        }
    }
}
