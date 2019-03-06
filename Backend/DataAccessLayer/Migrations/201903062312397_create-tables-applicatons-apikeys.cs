namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtablesapplicatonsapikeys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Url = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Logo = c.String(),
                        Description = c.String(),
                        UserDeletionUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApiKeys",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Key = c.String(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "PasswordSalt", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            
            AlterColumn("dbo.Users", "PasswordSalt", c => c.Binary());
            AlterColumn("dbo.Users", "PasswordHash", c => c.String());
            DropTable("dbo.ApiKeys");
            DropTable("dbo.Applications");
        }
    }
}
