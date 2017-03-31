namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedToDos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDo",
                c => new
                    {
                        ToDoID = c.Guid(nullable: false, identity: true),
                        AreaID = c.Guid(nullable: false),
                        ParentToDoID = c.Guid(),
                        Heading = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 200),
                        StartOn = c.DateTime(),
                        Completed = c.Boolean(nullable: false),
                        CompletedOn = c.DateTime(),
                        ListOrder = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ToDoID)
                .ForeignKey("dbo.Area", t => t.AreaID, cascadeDelete: true)
                .ForeignKey("dbo.ToDo", t => t.ParentToDoID)
                .Index(t => t.AreaID)
                .Index(t => t.ParentToDoID);
            
            CreateTable(
                "dbo.ToDoUser",
                c => new
                    {
                        ToDoID = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ToDoID, t.UserId })
                .ForeignKey("dbo.ToDo", t => t.ToDoID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ToDoID)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDo", "ParentToDoID", "dbo.ToDo");
            DropForeignKey("dbo.ToDoUser", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ToDoUser", "ToDoID", "dbo.ToDo");
            DropForeignKey("dbo.ToDo", "AreaID", "dbo.Area");
            DropIndex("dbo.ToDoUser", new[] { "UserId" });
            DropIndex("dbo.ToDoUser", new[] { "ToDoID" });
            DropIndex("dbo.ToDo", new[] { "ParentToDoID" });
            DropIndex("dbo.ToDo", new[] { "AreaID" });
            DropTable("dbo.ToDoUser");
            DropTable("dbo.ToDo");
        }
    }
}
