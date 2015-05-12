namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _AddedStatusChangedToArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Area", "StatusChanged", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Area", "StatusChanged");
        }
    }
}
