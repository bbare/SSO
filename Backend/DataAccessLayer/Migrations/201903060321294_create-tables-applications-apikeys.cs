namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtablesapplicationsapikeys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Url = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Logo = c.String(),
                        Description = c.String(),
                        UserDeletionUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Url);
            
            CreateTable(
                "dbo.ApiKeys",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        ApplicationUrl = c.String(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "PasswordSalt", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServiceName = c.String(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ServiceId = c.Guid(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Users", "PasswordSalt", c => c.Binary());
            AlterColumn("dbo.Users", "PasswordHash", c => c.String());
            DropTable("dbo.ApiKeys");
            DropTable("dbo.Applications");
        }
    }
}
