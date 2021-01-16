namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityDataHead_Modification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityDataHeads", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityDataHeads", "State");
        }
    }
}
