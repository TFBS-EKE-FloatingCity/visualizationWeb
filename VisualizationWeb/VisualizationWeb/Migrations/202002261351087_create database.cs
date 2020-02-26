namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SensorDatas",
                c => new
                    {
                        SensorDataID = c.Int(nullable: false, identity: true),
                        RealTime = c.DateTime(nullable: false),
                        SimulationTime = c.DateTime(nullable: false),
                        SensorID = c.Int(nullable: false),
                        SValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SensorDataID)
                .ForeignKey("dbo.Sensors", t => t.SensorID, cascadeDelete: true)
                .Index(t => t.SensorID);
            
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        SensorID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 200),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Einheiten = c.String(maxLength: 10),
                        SCode = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SensorID);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingID = c.Int(nullable: false, identity: true),
                        WindMax = c.Double(nullable: false),
                        SunMax = c.Double(nullable: false),
                        ConsumptionMax = c.Double(nullable: false),
                        WindActive = c.Boolean(nullable: false),
                        SunActive = c.Boolean(nullable: false),
                        ConsumptionActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SettingID);
            
            CreateTable(
                "dbo.SimDatas",
                c => new
                    {
                        SimDataID = c.Int(nullable: false, identity: true),
                        SimTypeID = c.Int(nullable: false),
                        SimTime = c.Time(nullable: false, precision: 7),
                        Wind = c.Double(nullable: false),
                        Sun = c.Double(nullable: false),
                        Consumption = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SimDataID)
                .ForeignKey("dbo.SimTypes", t => t.SimTypeID, cascadeDelete: true)
                .Index(t => t.SimTypeID);
            
            CreateTable(
                "dbo.SimTypes",
                c => new
                    {
                        SimTypeID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        SimFactor = c.Double(nullable: false),
                        Notes = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.SimTypeID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SimDatas", "SimTypeID", "dbo.SimTypes");
            DropForeignKey("dbo.SensorDatas", "SensorID", "dbo.Sensors");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.SimDatas", new[] { "SimTypeID" });
            DropIndex("dbo.SensorDatas", new[] { "SensorID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SimTypes");
            DropTable("dbo.SimDatas");
            DropTable("dbo.Settings");
            DropTable("dbo.Sensors");
            DropTable("dbo.SensorDatas");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
