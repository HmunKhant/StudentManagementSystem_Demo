namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuditLogKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsError = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        NRIC = c.String(),
                        Name = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.StudentSubjects",
                c => new
                    {
                        StudentId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.SubjectId })
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(),
                    })
                .PrimaryKey(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.StudentSubjects", "StudentId", "dbo.Students");
            DropIndex("dbo.StudentSubjects", new[] { "SubjectId" });
            DropIndex("dbo.StudentSubjects", new[] { "StudentId" });
            DropTable("dbo.Subjects");
            DropTable("dbo.StudentSubjects");
            DropTable("dbo.Students");
            DropTable("dbo.AuditLogs");
        }
    }
}
