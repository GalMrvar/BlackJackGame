namespace BlackjackDBLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBoolPlacedBet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turns", "PlacedBet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Turns", "PlacedBet");
        }
    }
}
