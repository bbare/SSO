namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtableservice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    ServiceName = c.String(nullable: false),
                    Disabled = c.Boolean(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Services");
        }
    }
}
