namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors");
            DropIndex("dbo.SensorDatas", new[] { "SensorID" });
            DropPrimaryKey("dbo.CityDatas");
            AddColumn("dbo.CityDatas", "UUID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CityDatas", "MesurementTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CityDatas", "SimulationID", c => c.Int());
            AddColumn("dbo.CityDatas", "WindMax", c => c.Int());
            AddColumn("dbo.CityDatas", "WindCurrent", c => c.Short());
            AddColumn("dbo.CityDatas", "SunMax", c => c.Int());
            AddColumn("dbo.CityDatas", "SunCurrent", c => c.Short());
            AddColumn("dbo.CityDatas", "ConsumptionMax", c => c.Int());
            AddColumn("dbo.CityDatas", "ConsumptionCurrent", c => c.Short());
            AddColumn("dbo.CityDatas", "SimulationActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CityDatas", "Simulationtime", c => c.DateTime());
            AddColumn("dbo.CityDatas", "TimeFactor", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CityDatas", "USonicInner1", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicOuter1", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "Pump1", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicInner2", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicOuter2", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "Pump2", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicInner3", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicOuter3", c => c.Short(nullable: false));
            AlterColumn("dbo.CityDatas", "Pump3", c => c.Short(nullable: false));
            AddPrimaryKey("dbo.CityDatas", "UUID");
            DropColumn("dbo.CityDatas", "CityDataID");
            DropTable("dbo.SensorDatas");
            DropTable("dbo.Sensors");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        SensorID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 200),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Einheiten = c.String(maxLength: 10),
                        SCode = c.Int(nullable: false),
                        Prefix = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SensorID);
            
            CreateTable(
                "dbo.SensorDatas",
                c => new
                    {
                        SensorDataID = c.Int(nullable: false, identity: true),
                        SensorID = c.Int(nullable: false),
                        MeasureTime = c.DateTime(nullable: false),
                        SValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SensorDataID);
            
            AddColumn("dbo.CityDatas", "CityDataID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.CityDatas");
            AlterColumn("dbo.CityDatas", "Pump3", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicOuter3", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicInner3", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "Pump2", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicOuter2", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicInner2", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "Pump1", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicOuter1", c => c.Int(nullable: false));
            AlterColumn("dbo.CityDatas", "USonicInner1", c => c.Int(nullable: false));
            DropColumn("dbo.CityDatas", "TimeFactor");
            DropColumn("dbo.CityDatas", "Simulationtime");
            DropColumn("dbo.CityDatas", "SimulationActive");
            DropColumn("dbo.CityDatas", "ConsumptionCurrent");
            DropColumn("dbo.CityDatas", "ConsumptionMax");
            DropColumn("dbo.CityDatas", "SunCurrent");
            DropColumn("dbo.CityDatas", "SunMax");
            DropColumn("dbo.CityDatas", "WindCurrent");
            DropColumn("dbo.CityDatas", "WindMax");
            DropColumn("dbo.CityDatas", "SimulationID");
            DropColumn("dbo.CityDatas", "MesurementTime");
            DropColumn("dbo.CityDatas", "UUID");
            AddPrimaryKey("dbo.CityDatas", "CityDataID");
            CreateIndex("dbo.SensorDatas", "SensorID");
            AddForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors", "SensorID", cascadeDelete: true);
        }
    }
}
