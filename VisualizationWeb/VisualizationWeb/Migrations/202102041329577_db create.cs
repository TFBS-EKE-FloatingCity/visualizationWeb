namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbcreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "browserConnectionString", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "browserConnectionString");
        }
    }
}
