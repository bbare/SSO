namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingmigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sessions", new[] { "UserId" });
            CreateIndex("dbo.Sessions", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sessions", new[] { "UserId" });
            CreateIndex("dbo.Sessions", "UserId");
        }
    }
}
