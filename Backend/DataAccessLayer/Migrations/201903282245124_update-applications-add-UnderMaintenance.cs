namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateapplicationsaddUnderMaintenance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "UnderMaintenance", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "UnderMaintenance");
        }
    }
}
