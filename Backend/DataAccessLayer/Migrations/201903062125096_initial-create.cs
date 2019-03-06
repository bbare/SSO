namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        PasswordHash = c.String(nullable: false),
                        PasswordSalt = c.Binary(nullable: false),
                        SecurityQ1 = c.String(),
                        SecurityQ1Answer = c.String(),
                        SecurityQ2 = c.String(),
                        SecurityQ2Answer = c.String(),
                        SecurityQ3 = c.String(),
                        SecurityQ3Answer = c.String(),
                        IncorrectPasswordCount = c.Int(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "UserId", "dbo.Users");
            DropIndex("dbo.Sessions", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Sessions");
        }
    }
}
