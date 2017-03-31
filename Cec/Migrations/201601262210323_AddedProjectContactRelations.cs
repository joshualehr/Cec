namespace Cec.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedProjectContactRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactProject", "Contact_ContactID", "dbo.Contact");
            DropForeignKey("dbo.ContactProject", "Project_ProjectID", "dbo.Project");
            DropIndex("dbo.ContactProject", new[] { "Contact_ContactID" });
            DropIndex("dbo.ContactProject", new[] { "Project_ProjectID" });
            DropTable("dbo.ContactProject");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ContactProject",
                c => new
                    {
                        Contact_ContactID = c.Guid(nullable: false),
                        Project_ProjectID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Contact_ContactID, t.Project_ProjectID });
            
            CreateIndex("dbo.ContactProject", "Project_ProjectID");
            CreateIndex("dbo.ContactProject", "Contact_ContactID");
            AddForeignKey("dbo.ContactProject", "Project_ProjectID", "dbo.Project", "ProjectID", cascadeDelete: true);
            AddForeignKey("dbo.ContactProject", "Contact_ContactID", "dbo.Contact", "ContactID", cascadeDelete: true);
        }
    }
}
