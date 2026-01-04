-- =============================================
-- Script: Seed Data
-- Description: Inserts initial data (Subjects: English, Math, Science)
-- =============================================

USE StudentDB;
GO

-- Insert Subjects if they don't exist
IF NOT EXISTS (SELECT * FROM [dbo].[Subjects] WHERE [SubjectName] = 'English')
BEGIN
    INSERT INTO [dbo].[Subjects] ([SubjectName]) VALUES ('English');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Subjects] WHERE [SubjectName] = 'Math')
BEGIN
    INSERT INTO [dbo].[Subjects] ([SubjectName]) VALUES ('Math');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Subjects] WHERE [SubjectName] = 'Science')
BEGIN
    INSERT INTO [dbo].[Subjects] ([SubjectName]) VALUES ('Science');
END
GO

PRINT 'Seed data inserted successfully!';
GO

