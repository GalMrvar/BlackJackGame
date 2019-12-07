namespace BlackjackDBLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoundAddedToTurn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turns", "Round_RoundId", c => c.Int());
            CreateIndex("dbo.Turns", "Round_RoundId");
            AddForeignKey("dbo.Turns", "Round_RoundId", "dbo.Rounds", "RoundId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turns", "Round_RoundId", "dbo.Rounds");
            DropIndex("dbo.Turns", new[] { "Round_RoundId" });
            DropColumn("dbo.Turns", "Round_RoundId");
        }
    }
}
