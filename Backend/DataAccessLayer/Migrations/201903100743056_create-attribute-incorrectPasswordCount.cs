namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createattributeincorrectPasswordCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IncorrectPasswordCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IncorrectPasswordCount");
        }
    }
}
