namespace CustomerInfoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "isPasswordChanged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isPasswordChanged");
        }
    }
}
