namespace WetherApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dependency : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CityDtoes", "temp", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CityDtoes", "temp", c => c.Double(nullable: false));
        }
    }
}
