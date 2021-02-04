namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityDataHeads",
                c => new
                    {
                        CityDataHeadID = c.Int(nullable: false, identity: true),
                        State = c.String(),
                        SimulationID = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CityDataHeadID);
            
            CreateTable(
                "dbo.CityDatas",
                c => new
                    {
                        UUID = c.String(nullable: false, maxLength: 128),
                        CityDataHeadID = c.Int(),
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
                        MesurementTime = c.DateTime(nullable: false),
                        SimulationID = c.Int(),
                        WindMax = c.Int(),
                        WindCurrent = c.Short(),
                        SunMax = c.Int(),
                        SunCurrent = c.Short(),
                        ConsumptionMax = c.Int(),
                        ConsumptionCurrent = c.Short(),
                        SimulationActive = c.Boolean(nullable: false),
                        Simulationtime = c.DateTime(),
                        TimeFactor = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UUID)
                .ForeignKey("dbo.CityDataHeads", t => t.CityDataHeadID)
                .Index(t => t.CityDataHeadID);
            
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
                        WindMax = c.Int(nullable: false),
                        SunMax = c.Int(nullable: false),
                        ConsumptionMax = c.Int(nullable: false),
                        rbPiConnectionString = c.String(maxLength: 500),
                        browserConnectionString = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.SettingID);
            
            CreateTable(
                "dbo.SimPositions",
                c => new
                    {
                        SimPositionID = c.Int(nullable: false, identity: true),
                        SunValue = c.Int(nullable: false),
                        WindValue = c.Int(nullable: false),
                        EnergyConsumptionValue = c.Int(nullable: false),
                        TimeRegistered = c.DateTime(nullable: false),
                        SimScenarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SimPositionID)
                .ForeignKey("dbo.SimScenarios", t => t.SimScenarioID, cascadeDelete: true)
                .Index(t => t.SimScenarioID);
            
            CreateTable(
                "dbo.SimScenarios",
                c => new
                    {
                        SimScenarioID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 500),
                        IsSimulationRunning = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SimScenarioID);
            
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
            DropForeignKey("dbo.SimPositions", "SimScenarioID", "dbo.SimScenarios");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CityDatas", "CityDataHeadID", "dbo.CityDataHeads");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.SimPositions", new[] { "SimScenarioID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.CityDatas", new[] { "CityDataHeadID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SimScenarios");
            DropTable("dbo.SimPositions");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.CityDatas");
            DropTable("dbo.CityDataHeads");
        }
    }
}
