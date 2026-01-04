namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailableDateToStudent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "AvailableDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "AvailableDate");
        }
    }
}

