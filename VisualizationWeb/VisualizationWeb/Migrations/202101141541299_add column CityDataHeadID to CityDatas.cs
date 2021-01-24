namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnCityDataHeadIDtoCityDatas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityDatas", "CityDataHeadID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityDatas", "CityDataHeadID");
        }
    }
}
