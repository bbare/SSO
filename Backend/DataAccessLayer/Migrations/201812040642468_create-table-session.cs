namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtablesession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Token = c.String(nullable: false),
                    ExpiresAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UserId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sessions", new[] { "UserId" });
            DropForeignKey("dbo.Sessions", "UserId", "dbo.Users");
            DropTable("dbo.Sessions");
        }
    }
}
