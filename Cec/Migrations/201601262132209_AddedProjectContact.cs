namespace Cec.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedProjectContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectContact",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false),
                        ContactID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.ContactID })
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.ContactID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectContact", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectContact", "ContactID", "dbo.Contact");
            DropIndex("dbo.ProjectContact", new[] { "ContactID" });
            DropIndex("dbo.ProjectContact", new[] { "ProjectID" });
            DropTable("dbo.ProjectContact");
        }
    }
}
