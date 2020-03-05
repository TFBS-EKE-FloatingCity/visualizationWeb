namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatesimtypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors");
            DropPrimaryKey("dbo.Sensors");
            AddColumn("dbo.SimTypes", "StartTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.SimTypes", "Interval", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.SimTypes", "EndTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Sensors", "SensorID", c => c.Int(nullable: false));
            AlterColumn("dbo.SimDatas", "SimTime", c => c.Time(nullable: false, precision: 7));
            AddPrimaryKey("dbo.Sensors", "SensorID");
            AddForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors", "SensorID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors");
            DropPrimaryKey("dbo.Sensors");
            AlterColumn("dbo.SimDatas", "SimTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sensors", "SensorID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.SimTypes", "EndTime");
            DropColumn("dbo.SimTypes", "Interval");
            DropColumn("dbo.SimTypes", "StartTime");
            AddPrimaryKey("dbo.Sensors", "SensorID");
            AddForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors", "SensorID", cascadeDelete: true);
        }
    }
}
