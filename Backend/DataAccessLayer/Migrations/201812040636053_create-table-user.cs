namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtableuser : DbMigration
    {
        public override void Up()
        {
            
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
                        Disabled = c.Boolean(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");

        }
    }
}
