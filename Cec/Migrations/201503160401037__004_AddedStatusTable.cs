namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _004_AddedStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Guid(nullable: false, identity: true),
                        Designation = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.StatusId);
            
            AddColumn("dbo.Area", "StatusId", c => c.Guid());
            CreateIndex("dbo.Area", "StatusId");
            AddForeignKey("dbo.Area", "StatusId", "dbo.Status", "StatusId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Area", "StatusId", "dbo.Status");
            DropIndex("dbo.Area", new[] { "StatusId" });
            DropColumn("dbo.Area", "StatusId");
            DropTable("dbo.Status");
        }
    }
}
