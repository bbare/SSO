namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedclientUseraddedClientIdinUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientUsers", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientUsers", "UserId", "dbo.Users");
            DropIndex("dbo.ClientUsers", new[] { "ClientId" });
            DropIndex("dbo.ClientUsers", new[] { "UserId" });
            AddColumn("dbo.Users", "ClientId", c => c.Guid());
            CreateIndex("dbo.Users", "ClientId");
            AddForeignKey("dbo.Users", "ClientId", "dbo.Clients", "Id");
            DropTable("dbo.ClientUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClientUsers",
                c => new
                    {
                        ClientId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => new { t.ClientId, t.UserId });
            
            DropForeignKey("dbo.Users", "ClientId", "dbo.Clients");
            DropIndex("dbo.Users", new[] { "ClientId" });
            DropColumn("dbo.Users", "ClientId");
            CreateIndex("dbo.ClientUsers", "UserId");
            CreateIndex("dbo.ClientUsers", "ClientId");
            AddForeignKey("dbo.ClientUsers", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientUsers", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
    }
}
