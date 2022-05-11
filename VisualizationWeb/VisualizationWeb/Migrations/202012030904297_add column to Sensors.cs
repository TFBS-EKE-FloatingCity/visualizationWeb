namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumntoSensors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sensors", "GroupNr", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sensors", "GroupNr");
        }
    }
}
