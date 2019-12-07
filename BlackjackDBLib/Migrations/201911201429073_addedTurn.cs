namespace BlackjackDBLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTurn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rounds", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Rounds", "Result_ResultId", "dbo.Results");
            DropIndex("dbo.Rounds", new[] { "Player_PlayerId" });
            DropIndex("dbo.Rounds", new[] { "Result_ResultId" });
            CreateTable(
                "dbo.Turns",
                c => new
                    {
                        TurnId = c.Int(nullable: false, identity: true),
                        Bet = c.Int(nullable: false),
                        Player_PlayerId = c.Int(),
                        Result_ResultId = c.Int(),
                    })
                .PrimaryKey(t => t.TurnId)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Result_ResultId);
            
            DropColumn("dbo.Rounds", "Bet");
            DropColumn("dbo.Rounds", "Player_PlayerId");
            DropColumn("dbo.Rounds", "Result_ResultId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rounds", "Result_ResultId", c => c.Int());
            AddColumn("dbo.Rounds", "Player_PlayerId", c => c.Int());
            AddColumn("dbo.Rounds", "Bet", c => c.Int(nullable: false));
            DropForeignKey("dbo.Turns", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.Turns", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.Turns", new[] { "Result_ResultId" });
            DropIndex("dbo.Turns", new[] { "Player_PlayerId" });
            DropTable("dbo.Turns");
            CreateIndex("dbo.Rounds", "Result_ResultId");
            CreateIndex("dbo.Rounds", "Player_PlayerId");
            AddForeignKey("dbo.Rounds", "Result_ResultId", "dbo.Results", "ResultId");
            AddForeignKey("dbo.Rounds", "Player_PlayerId", "dbo.Players", "PlayerId");
        }
    }
}
