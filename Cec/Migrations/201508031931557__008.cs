namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _008 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.AspNetUserClaim",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ClaimType = c.String(),
            //            ClaimValue = c.String(),
            //            User_Id = c.String(),
            //            AspNetUser_Id = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUser", t => t.AspNetUser_Id)
            //    .Index(t => t.AspNetUser_Id);
            
            //CreateTable(
            //    "dbo.AspNetUser",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            UserName = c.String(),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            Discriminator = c.String(),
            //            ContactID = c.Guid(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.AspNetRole",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.AspNetUserLogin",
            //    c => new
            //        {
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            LoginProvider = c.String(nullable: false, maxLength: 128),
            //            ProviderKey = c.String(),
            //            AspNetUser_Id = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.UserId, t.LoginProvider })
            //    .ForeignKey("dbo.AspNetUser", t => t.AspNetUser_Id)
            //    .Index(t => t.AspNetUser_Id);
            
            //CreateTable(
            //    "dbo.AspNetRoleAspNetUser",
            //    c => new
            //        {
            //            AspNetRole_Id = c.String(nullable: false, maxLength: 128),
            //            AspNetUser_Id = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.AspNetRole_Id, t.AspNetUser_Id })
            //    .ForeignKey("dbo.AspNetRole", t => t.AspNetRole_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetUser", t => t.AspNetUser_Id, cascadeDelete: true)
            //    .Index(t => t.AspNetRole_Id)
            //    .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.AspNetUserLogin", "AspNetUser_Id", "dbo.AspNetUser");
            //DropForeignKey("dbo.AspNetUserClaim", "AspNetUser_Id", "dbo.AspNetUser");
            //DropForeignKey("dbo.AspNetRoleAspNetUser", "AspNetUser_Id", "dbo.AspNetUser");
            //DropForeignKey("dbo.AspNetRoleAspNetUser", "AspNetRole_Id", "dbo.AspNetRole");
            //DropIndex("dbo.AspNetRoleAspNetUser", new[] { "AspNetUser_Id" });
            //DropIndex("dbo.AspNetRoleAspNetUser", new[] { "AspNetRole_Id" });
            //DropIndex("dbo.AspNetUserLogin", new[] { "AspNetUser_Id" });
            //DropIndex("dbo.AspNetUserClaim", new[] { "AspNetUser_Id" });
            //DropTable("dbo.AspNetRoleAspNetUser");
            //DropTable("dbo.AspNetUserLogin");
            //DropTable("dbo.AspNetRole");
            //DropTable("dbo.AspNetUser");
            //DropTable("dbo.AspNetUserClaim");
        }
    }
}
