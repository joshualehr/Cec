namespace Cec.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PhaseQuantities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AreaMaterial", "RoughQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.AreaMaterial", "FinishQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ModelMaterial", "RoughQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ModelMaterial", "FinishQuantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModelMaterial", "FinishQuantity");
            DropColumn("dbo.ModelMaterial", "RoughQuantity");
            DropColumn("dbo.AreaMaterial", "FinishQuantity");
            DropColumn("dbo.AreaMaterial", "RoughQuantity");
        }
    }
}
