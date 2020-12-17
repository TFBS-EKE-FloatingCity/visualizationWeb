namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SimScenarios", "TimeFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SimScenarios", "TimeFactor", c => c.Int(nullable: false));
        }
    }
}
