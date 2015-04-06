namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _006_AddUserIdToProject : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Project", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Project", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Project", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Project", name: "UserId", newName: "User_Id");
        }
    }
}
