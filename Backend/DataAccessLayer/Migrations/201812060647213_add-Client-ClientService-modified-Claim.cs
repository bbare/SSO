namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addClientClientServicemodifiedClaim : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        Address = c.String(),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientUsers",
                c => new
                    {
                        ClientId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => new { t.ClientId, t.UserId })
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.ClientUsers", "ClientId", "dbo.Clients");
            DropIndex("dbo.ClientUsers", new[] { "UserId" });
            DropIndex("dbo.ClientUsers", new[] { "ClientId" });
            DropTable("dbo.ClientUsers");
            DropTable("dbo.Clients");
        }
    }
}
