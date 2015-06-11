namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _007_addedListOrderToStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Status", "ListOrder", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Status", "ListOrder");
        }
    }
}
