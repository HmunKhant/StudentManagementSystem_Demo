namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StudentManagementSystem.Models.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentManagementSystem.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StudentManagementSystem.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  Seed Subjects: English, Math, Science
            context.Subjects.AddOrUpdate(
                s => s.SubjectName,
                new Subject { SubjectName = "English" },
                new Subject { SubjectName = "Math" },
                new Subject { SubjectName = "Science" }
            );

            context.SaveChanges();
        }
    }
}
