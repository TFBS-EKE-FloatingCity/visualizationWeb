namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settingaddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "rbPiConnectionString", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "rbPiConnectionString");
        }
    }
}
