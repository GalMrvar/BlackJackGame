namespace BlackjackDBLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Money = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerId);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        Win = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ResultId);
            
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        RoundId = c.Int(nullable: false, identity: true),
                        Bet = c.Int(nullable: false),
                        Player_PlayerId = c.Int(),
                        Result_ResultId = c.Int(),
                    })
                .PrimaryKey(t => t.RoundId)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Result_ResultId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rounds", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.Rounds", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.Rounds", new[] { "Result_ResultId" });
            DropIndex("dbo.Rounds", new[] { "Player_PlayerId" });
            DropTable("dbo.Rounds");
            DropTable("dbo.Results");
            DropTable("dbo.Players");
        }
    }
}
