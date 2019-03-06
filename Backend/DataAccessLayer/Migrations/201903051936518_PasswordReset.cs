namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordReset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetIDs",
                c => new
                {
                    PasswordResetID = c.Guid(nullable: false),
                    ResetToken = c.String(nullable: false),
                    UserID = c.Guid(nullable: false),
                    ExpirationTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ResetCount = c.Int(nullable: false),
                    Disabled = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.PasswordResetID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ResetIDs", new[] { "UserId" });
            DropForeignKey("dbo.ResetIDs", "UserId", "dbo.Users");
            DropTable("dbo.ResetIDs");
        }
    }
}
