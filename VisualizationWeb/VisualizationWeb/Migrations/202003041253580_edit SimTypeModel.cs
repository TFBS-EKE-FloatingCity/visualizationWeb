namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editSimTypeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SimTypes", "StartTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.SimTypes", "Interval", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.SimTypes", "EndTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SimTypes", "EndTime");
            DropColumn("dbo.SimTypes", "Interval");
            DropColumn("dbo.SimTypes", "StartTime");
        }
    }
}
