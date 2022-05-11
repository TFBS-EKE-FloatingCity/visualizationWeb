namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removelastaddedcolumnofSensorstable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sensors", "GroupNr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sensors", "GroupNr", c => c.Int());
        }
    }
}
