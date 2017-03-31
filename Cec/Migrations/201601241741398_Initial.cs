namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaMaterial",
                c => new
                    {
                        AreaID = c.Guid(nullable: false),
                        MaterialID = c.Guid(nullable: false),
                        Quantity = c.Double(nullable: false),
                        UnitOfMeasure_UnitOfMeasureID = c.Guid(),
                    })
                .PrimaryKey(t => new { t.AreaID, t.MaterialID })
                .ForeignKey("dbo.Area", t => t.AreaID, cascadeDelete: true)
                .ForeignKey("dbo.Material", t => t.MaterialID, cascadeDelete: true)
                .ForeignKey("dbo.UnitOfMeasure", t => t.UnitOfMeasure_UnitOfMeasureID)
                .Index(t => t.AreaID)
                .Index(t => t.MaterialID)
                .Index(t => t.UnitOfMeasure_UnitOfMeasureID);
            
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        AreaID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        Address = c.String(maxLength: 100),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 2),
                        PostalCode = c.Int(),
                        BuildingID = c.Guid(nullable: false),
                        ModelID = c.Guid(),
                        StatusId = c.Guid(nullable: false),
                        StatusChanged = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AreaID)
                .ForeignKey("dbo.Building", t => t.BuildingID, cascadeDelete: true)
                .ForeignKey("dbo.Model", t => t.ModelID)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.BuildingID)
                .Index(t => t.ModelID)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Building",
                c => new
                    {
                        BuildingID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        Address = c.String(maxLength: 100),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 2),
                        PostalCode = c.Int(),
                        ProjectID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.BuildingID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        DocumentID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        FileLink = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentID);
            
            CreateTable(
                "dbo.Model",
                c => new
                    {
                        ModelID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        ProjectID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ModelID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.ModelMaterial",
                c => new
                    {
                        ModelID = c.Guid(nullable: false),
                        MaterialID = c.Guid(nullable: false),
                        Quantity = c.Double(nullable: false),
                        UnitOfMeasure_UnitOfMeasureID = c.Guid(),
                    })
                .PrimaryKey(t => new { t.ModelID, t.MaterialID })
                .ForeignKey("dbo.Material", t => t.MaterialID, cascadeDelete: true)
                .ForeignKey("dbo.UnitOfMeasure", t => t.UnitOfMeasure_UnitOfMeasureID)
                .ForeignKey("dbo.Model", t => t.ModelID, cascadeDelete: true)
                .Index(t => t.ModelID)
                .Index(t => t.MaterialID)
                .Index(t => t.UnitOfMeasure_UnitOfMeasureID);
            
            CreateTable(
                "dbo.Material",
                c => new
                    {
                        MaterialID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        ImagePath = c.String(),
                        Barcode = c.Int(),
                        UnitOfMeasureID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MaterialID)
                .ForeignKey("dbo.UnitOfMeasure", t => t.UnitOfMeasureID, cascadeDelete: true)
                .Index(t => t.UnitOfMeasureID);
            
            CreateTable(
                "dbo.UnitOfMeasure",
                c => new
                    {
                        UnitOfMeasureID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UnitOfMeasureID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        UserId = c.String(maxLength: 128),
                        PurchaseOrder = c.String(maxLength: 20),
                        Address = c.String(maxLength: 100),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 2),
                        PostalCode = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactID = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        Trade = c.String(maxLength: 50),
                        Phone = c.String(),
                        Email = c.String(),
                        Chat = c.String(),
                        Website = c.String(),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ContactID = c.Guid(nullable: false),
                        AllRoles = c.String(),
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
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.ContactID)
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 20),
                        ListOrder = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.AspNetUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUser", t => t.AspNetUser_Id)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.AspNetUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(),
                        ContactID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider })
                .ForeignKey("dbo.AspNetUser", t => t.AspNetUser_Id)
                .Index(t => t.AspNetUser_Id);
            
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
                "dbo.DocumentArea",
                c => new
                    {
                        Document_DocumentID = c.Guid(nullable: false),
                        Area_AreaID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Document_DocumentID, t.Area_AreaID })
                .ForeignKey("dbo.Document", t => t.Document_DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.Area", t => t.Area_AreaID, cascadeDelete: true)
                .Index(t => t.Document_DocumentID)
                .Index(t => t.Area_AreaID);
            
            CreateTable(
                "dbo.DocumentBuilding",
                c => new
                    {
                        Document_DocumentID = c.Guid(nullable: false),
                        Building_BuildingID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Document_DocumentID, t.Building_BuildingID })
                .ForeignKey("dbo.Document", t => t.Document_DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.Building", t => t.Building_BuildingID, cascadeDelete: true)
                .Index(t => t.Document_DocumentID)
                .Index(t => t.Building_BuildingID);
            
            CreateTable(
                "dbo.ModelDocument",
                c => new
                    {
                        Model_ModelID = c.Guid(nullable: false),
                        Document_DocumentID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Model_ModelID, t.Document_DocumentID })
                .ForeignKey("dbo.Model", t => t.Model_ModelID, cascadeDelete: true)
                .ForeignKey("dbo.Document", t => t.Document_DocumentID, cascadeDelete: true)
                .Index(t => t.Model_ModelID)
                .Index(t => t.Document_DocumentID);
            
            CreateTable(
                "dbo.ContactProject",
                c => new
                    {
                        Contact_ContactID = c.Guid(nullable: false),
                        Project_ProjectID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Contact_ContactID, t.Project_ProjectID })
                .ForeignKey("dbo.Contact", t => t.Contact_ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.Project_ProjectID, cascadeDelete: true)
                .Index(t => t.Contact_ContactID)
                .Index(t => t.Project_ProjectID);
            
            CreateTable(
                "dbo.ProjectDocument",
                c => new
                    {
                        Project_ProjectID = c.Guid(nullable: false),
                        Document_DocumentID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_ProjectID, t.Document_DocumentID })
                .ForeignKey("dbo.Project", t => t.Project_ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Document", t => t.Document_DocumentID, cascadeDelete: true)
                .Index(t => t.Project_ProjectID)
                .Index(t => t.Document_DocumentID);
            
            CreateTable(
                "dbo.AspNetRoleAspNetUser",
                c => new
                    {
                        AspNetRole_Id = c.String(nullable: false, maxLength: 128),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AspNetRole_Id, t.AspNetUser_Id })
                .ForeignKey("dbo.AspNetRole", t => t.AspNetRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUser", t => t.AspNetUser_Id, cascadeDelete: true)
                .Index(t => t.AspNetRole_Id)
                .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogin", "AspNetUser_Id", "dbo.AspNetUser");
            DropForeignKey("dbo.AspNetUserClaim", "AspNetUser_Id", "dbo.AspNetUser");
            DropForeignKey("dbo.AspNetRoleAspNetUser", "AspNetUser_Id", "dbo.AspNetUser");
            DropForeignKey("dbo.AspNetRoleAspNetUser", "AspNetRole_Id", "dbo.AspNetRole");
            DropForeignKey("dbo.Area", "StatusId", "dbo.Status");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Project", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Model", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectDocument", "Document_DocumentID", "dbo.Document");
            DropForeignKey("dbo.ProjectDocument", "Project_ProjectID", "dbo.Project");
            DropForeignKey("dbo.ContactProject", "Project_ProjectID", "dbo.Project");
            DropForeignKey("dbo.ContactProject", "Contact_ContactID", "dbo.Contact");
            DropForeignKey("dbo.Building", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ModelMaterial", "ModelID", "dbo.Model");
            DropForeignKey("dbo.Material", "UnitOfMeasureID", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.ModelMaterial", "UnitOfMeasure_UnitOfMeasureID", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.AreaMaterial", "UnitOfMeasure_UnitOfMeasureID", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.ModelMaterial", "MaterialID", "dbo.Material");
            DropForeignKey("dbo.AreaMaterial", "MaterialID", "dbo.Material");
            DropForeignKey("dbo.ModelDocument", "Document_DocumentID", "dbo.Document");
            DropForeignKey("dbo.ModelDocument", "Model_ModelID", "dbo.Model");
            DropForeignKey("dbo.Area", "ModelID", "dbo.Model");
            DropForeignKey("dbo.DocumentBuilding", "Building_BuildingID", "dbo.Building");
            DropForeignKey("dbo.DocumentBuilding", "Document_DocumentID", "dbo.Document");
            DropForeignKey("dbo.DocumentArea", "Area_AreaID", "dbo.Area");
            DropForeignKey("dbo.DocumentArea", "Document_DocumentID", "dbo.Document");
            DropForeignKey("dbo.Area", "BuildingID", "dbo.Building");
            DropForeignKey("dbo.AreaMaterial", "AreaID", "dbo.Area");
            DropIndex("dbo.AspNetRoleAspNetUser", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetRoleAspNetUser", new[] { "AspNetRole_Id" });
            DropIndex("dbo.ProjectDocument", new[] { "Document_DocumentID" });
            DropIndex("dbo.ProjectDocument", new[] { "Project_ProjectID" });
            DropIndex("dbo.ContactProject", new[] { "Project_ProjectID" });
            DropIndex("dbo.ContactProject", new[] { "Contact_ContactID" });
            DropIndex("dbo.ModelDocument", new[] { "Document_DocumentID" });
            DropIndex("dbo.ModelDocument", new[] { "Model_ModelID" });
            DropIndex("dbo.DocumentBuilding", new[] { "Building_BuildingID" });
            DropIndex("dbo.DocumentBuilding", new[] { "Document_DocumentID" });
            DropIndex("dbo.DocumentArea", new[] { "Area_AreaID" });
            DropIndex("dbo.DocumentArea", new[] { "Document_DocumentID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserLogin", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserClaim", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "ContactID" });
            DropIndex("dbo.Project", new[] { "UserId" });
            DropIndex("dbo.Material", new[] { "UnitOfMeasureID" });
            DropIndex("dbo.ModelMaterial", new[] { "UnitOfMeasure_UnitOfMeasureID" });
            DropIndex("dbo.ModelMaterial", new[] { "MaterialID" });
            DropIndex("dbo.ModelMaterial", new[] { "ModelID" });
            DropIndex("dbo.Model", new[] { "ProjectID" });
            DropIndex("dbo.Building", new[] { "ProjectID" });
            DropIndex("dbo.Area", new[] { "StatusId" });
            DropIndex("dbo.Area", new[] { "ModelID" });
            DropIndex("dbo.Area", new[] { "BuildingID" });
            DropIndex("dbo.AreaMaterial", new[] { "UnitOfMeasure_UnitOfMeasureID" });
            DropIndex("dbo.AreaMaterial", new[] { "MaterialID" });
            DropIndex("dbo.AreaMaterial", new[] { "AreaID" });
            DropTable("dbo.AspNetRoleAspNetUser");
            DropTable("dbo.ProjectDocument");
            DropTable("dbo.ContactProject");
            DropTable("dbo.ModelDocument");
            DropTable("dbo.DocumentBuilding");
            DropTable("dbo.DocumentArea");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserLogin");
            DropTable("dbo.AspNetRole");
            DropTable("dbo.AspNetUser");
            DropTable("dbo.AspNetUserClaim");
            DropTable("dbo.Status");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Contact");
            DropTable("dbo.Project");
            DropTable("dbo.UnitOfMeasure");
            DropTable("dbo.Material");
            DropTable("dbo.ModelMaterial");
            DropTable("dbo.Model");
            DropTable("dbo.Document");
            DropTable("dbo.Building");
            DropTable("dbo.Area");
            DropTable("dbo.AreaMaterial");
        }
    }
}
