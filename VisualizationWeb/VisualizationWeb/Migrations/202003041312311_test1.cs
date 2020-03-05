namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors");
            DropPrimaryKey("dbo.Sensors");
            AlterColumn("dbo.Sensors", "SensorID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Sensors", "SensorID");
            AddForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors", "SensorID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors");
            DropPrimaryKey("dbo.Sensors");
            AlterColumn("dbo.Sensors", "SensorID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Sensors", "SensorID");
            AddForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors", "SensorID", cascadeDelete: true);
        }
    }
}
