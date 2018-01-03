namespace QuizWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        PlayerId = c.String(nullable: false, maxLength: 32),
                        ChosenOptionIndex = c.Int(nullable: false),
                        AssignedValue = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.Question_QuestionId, cascadeDelete: true);
            CreateIndex("dbo.Answers", new[] { "PlayerId", "Question_QuestionId" }, unique: true, name: "IX_Answer");

            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        RoundId = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        OwnerUserId = c.String(),
                        Body = c.String(nullable: false),
                        Option1 = c.String(nullable: false),
                        Option2 = c.String(nullable: false),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        IndexOfCorrectOption = c.Int(nullable: false),
                        Comment = c.String(),
                        CreateAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId);
            
            CreateTable(
                "dbo.Contexts",
                c => new
                    {
                        ContextId = c.Int(nullable: false, identity: true),
                        CurrentState = c.Int(nullable: false),
                        IsDashboardAvailableForUsers = c.Boolean(nullable: false),
                        ShowAssignedValueInDashboard = c.Boolean(nullable: false),
                        CurrentQuestion_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.ContextId)
                .ForeignKey("dbo.Questions", t => t.CurrentQuestion_QuestionId)
                .Index(t => t.CurrentQuestion_QuestionId);
            
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        RoundId = c.Int(nullable: false, identity: true),
                        SortOrder = c.Int(nullable: false),
                        OwnerUserId = c.String(),
                        Title = c.String(nullable: false),
                        CreateAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RoundId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedAt = c.DateTime(),
                        IsAdmin = c.Boolean(nullable: false),
                        Pass = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Name, unique: true, name: "IX_User_Name");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contexts", "CurrentQuestion_QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "Question_QuestionId", "dbo.Questions");
            DropIndex("dbo.Users", "IX_User_Name");
            DropIndex("dbo.Contexts", new[] { "CurrentQuestion_QuestionId" });
            DropIndex("dbo.Answers", "IX_Answer");
            DropTable("dbo.Users");
            DropTable("dbo.Rounds");
            DropTable("dbo.Contexts");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
