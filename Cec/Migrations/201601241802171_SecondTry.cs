namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondTry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetRoleAspNetUser", "AspNetRole_Id", "dbo.AspNetRole");
            DropForeignKey("dbo.AspNetRoleAspNetUser", "AspNetUser_Id", "dbo.AspNetUser");
            DropForeignKey("dbo.AspNetUserClaim", "AspNetUser_Id", "dbo.AspNetUser");
            DropForeignKey("dbo.AspNetUserLogin", "AspNetUser_Id", "dbo.AspNetUser");
            DropIndex("dbo.AspNetUserClaim", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserLogin", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetRoleAspNetUser", new[] { "AspNetRole_Id" });
            DropIndex("dbo.AspNetRoleAspNetUser", new[] { "AspNetUser_Id" });
            DropTable("dbo.AspNetUserClaim");
            DropTable("dbo.AspNetUser");
            DropTable("dbo.AspNetRole");
            DropTable("dbo.AspNetUserLogin");
            DropTable("dbo.AspNetRoleAspNetUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AspNetRoleAspNetUser",
                c => new
                    {
                        AspNetRole_Id = c.String(nullable: false, maxLength: 128),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AspNetRole_Id, t.AspNetUser_Id });
            
            CreateTable(
                "dbo.AspNetUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider });
            
            CreateTable(
                "dbo.AspNetRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.AspNetUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.AspNetRoleAspNetUser", "AspNetUser_Id");
            CreateIndex("dbo.AspNetRoleAspNetUser", "AspNetRole_Id");
            CreateIndex("dbo.AspNetUserLogin", "AspNetUser_Id");
            CreateIndex("dbo.AspNetUserClaim", "AspNetUser_Id");
            AddForeignKey("dbo.AspNetUserLogin", "AspNetUser_Id", "dbo.AspNetUser", "Id");
            AddForeignKey("dbo.AspNetUserClaim", "AspNetUser_Id", "dbo.AspNetUser", "Id");
            AddForeignKey("dbo.AspNetRoleAspNetUser", "AspNetUser_Id", "dbo.AspNetUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetRoleAspNetUser", "AspNetRole_Id", "dbo.AspNetRole", "Id", cascadeDelete: true);
        }
    }
}
