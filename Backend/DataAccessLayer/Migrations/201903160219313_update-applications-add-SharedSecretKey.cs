namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateapplicationsaddSharedSecretKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "SharedSecretKey", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "SharedSecretKey");
        }
    }
}
