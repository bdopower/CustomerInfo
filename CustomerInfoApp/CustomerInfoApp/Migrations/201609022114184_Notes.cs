namespace CustomerInfoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Notes");
        }
    }
}
