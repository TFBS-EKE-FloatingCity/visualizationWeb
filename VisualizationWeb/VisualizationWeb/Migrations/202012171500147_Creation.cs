namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Creation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityDatas",
                c => new
                    {
                        CityDataID = c.Int(nullable: false, identity: true),
                        USonicInner1 = c.Short(nullable: false),
                        USonicOuter1 = c.Short(nullable: false),
                        Pump1 = c.Short(nullable: false),
                        USonicInner2 = c.Short(nullable: false),
                        USonicOuter2 = c.Short(nullable: false),
                        Pump2 = c.Short(nullable: false),
                        USonicInner3 = c.Short(nullable: false),
                        USonicOuter3 = c.Short(nullable: false),
                        Pump3 = c.Short(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        SimulationID = c.Int(),
                        WindMax = c.Int(),
                        WindCurrent = c.Byte(),
                        SunMax = c.Int(),
                        SunCurrent = c.Byte(),
                        ConsumptionMax = c.Int(),
                        ConsumptionCurrent = c.Byte(),
                        SimulationActive = c.Boolean(nullable: false),
                        Simulationtime = c.DateTime(),
                        TimeFactor = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CityDataID);
            
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
                        SimTime = c.DateTime(nullable: false),
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
                        StartTime = c.DateTime(nullable: false),
                        Interval = c.Time(nullable: false, precision: 7),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SimTypeID);
            
            CreateTable(
                "dbo.SimulationHistories",
                c => new
                    {
                        HistoryID = c.Int(nullable: false, identity: true),
                        RealStartTime = c.DateTime(nullable: false),
                        SimTypeID = c.Int(nullable: false),
                        Canceled = c.DateTime(),
                    })
                .PrimaryKey(t => t.HistoryID)
                .ForeignKey("dbo.SimTypes", t => t.SimTypeID, cascadeDelete: true)
                .Index(t => t.SimTypeID);
            
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
            DropForeignKey("dbo.SimulationHistories", "SimTypeID", "dbo.SimTypes");
            DropForeignKey("dbo.SimDatas", "SimTypeID", "dbo.SimTypes");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.SimulationHistories", new[] { "SimTypeID" });
            DropIndex("dbo.SimDatas", new[] { "SimTypeID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SimulationHistories");
            DropTable("dbo.SimTypes");
            DropTable("dbo.SimDatas");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.CityDatas");
        }
    }
}
