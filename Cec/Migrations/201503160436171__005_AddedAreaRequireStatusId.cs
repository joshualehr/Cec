namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _005_AddedAreaRequireStatusId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Area", "StatusId", "dbo.Status");
            DropIndex("dbo.Area", new[] { "StatusId" });
            AlterColumn("dbo.Area", "StatusId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Area", "StatusId");
            AddForeignKey("dbo.Area", "StatusId", "dbo.Status", "StatusId", cascadeDelete: true);
            DropColumn("dbo.Area", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Area", "Status", c => c.String(nullable: false, maxLength: 20));
            DropForeignKey("dbo.Area", "StatusId", "dbo.Status");
            DropIndex("dbo.Area", new[] { "StatusId" });
            AlterColumn("dbo.Area", "StatusId", c => c.Guid());
            CreateIndex("dbo.Area", "StatusId");
            AddForeignKey("dbo.Area", "StatusId", "dbo.Status", "StatusId");
        }
    }
}
