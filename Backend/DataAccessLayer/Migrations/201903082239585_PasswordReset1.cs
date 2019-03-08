namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordReset1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordResets",
                c => new
                    {
                        PasswordResetID = c.Guid(nullable: false),
                        ResetToken = c.String(nullable: false),
                        UserID = c.Guid(nullable: false),
                        ExpirationTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ResetCount = c.Int(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        AllowPasswordReset = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PasswordResetID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PasswordResets", "UserID", "dbo.Users");
            DropIndex("dbo.PasswordResets", new[] { "UserID" });
            DropTable("dbo.PasswordResets");
        }
    }
}
