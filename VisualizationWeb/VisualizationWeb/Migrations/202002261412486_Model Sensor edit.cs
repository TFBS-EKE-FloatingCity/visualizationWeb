namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelSensoredit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sensors", "SCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sensors", "SCode", c => c.String(maxLength: 3));
        }
    }
}
