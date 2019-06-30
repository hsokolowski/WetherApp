namespace WetherApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rebase : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "country", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cities", "country", c => c.String(maxLength: 3));
        }
    }
}
