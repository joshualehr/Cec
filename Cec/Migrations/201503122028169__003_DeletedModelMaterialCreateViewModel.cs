namespace Cec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _003_DeletedModelMaterialCreateViewModel : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ModelMaterialCreateItemViewModel");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ModelMaterialCreateItemViewModel",
                c => new
                    {
                        ModelId = c.Guid(nullable: false),
                        MaterialId = c.Guid(nullable: false),
                        MaterialName = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                        UnitOfMeasureName = c.String(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModelId, t.MaterialId });
            
        }
    }
}
