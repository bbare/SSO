namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useraddmanager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ManagerId", c => c.Guid());
            CreateIndex("dbo.Users", "ManagerId");
            AddForeignKey("dbo.Users", "ManagerId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ManagerId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "ManagerId" });
            DropColumn("dbo.Users", "ManagerId");
        }
    }
}
