namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtableclaim : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claims",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    UserId = c.Guid(nullable: false),
                    ServiceId = c.Guid(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ServiceId);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Claims", new[] { "ServiceId" });
            DropIndex("dbo.Claims", new[] { "UserId" });
            DropForeignKey("dbo.Claims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Claims", "ServiceId", "dbo.Services");
            DropTable("dbo.Claims");
        }
    }
}
