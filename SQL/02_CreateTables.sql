-- =============================================
-- Script: Create Tables
-- Description: Creates all required tables for Student Management System
-- =============================================

USE StudentDB;
GO

-- Create Subjects Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subjects]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Subjects] (
        [SubjectId] INT IDENTITY(1,1) PRIMARY KEY,
        [SubjectName] NVARCHAR(100) NOT NULL
    );
END
GO

-- Create Students Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Students] (
        [StudentId] INT IDENTITY(1,1) PRIMARY KEY,
        [NRIC] NVARCHAR(20) NOT NULL UNIQUE,
        [Name] NVARCHAR(100) NOT NULL,
        [Gender] NVARCHAR(1) NOT NULL CHECK ([Gender] IN ('M', 'F')),
        [DateOfBirth] DATETIME NOT NULL,
        [AvailableDate] DATETIME NULL
    );
    
    -- Create Index on NRIC for faster searches
    CREATE INDEX IX_Students_NRIC ON [dbo].[Students]([NRIC]);
    CREATE INDEX IX_Students_Name ON [dbo].[Students]([Name]);
END
GO

-- Add AvailableDate column if table exists but column doesn't
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = 'AvailableDate')
BEGIN
    ALTER TABLE [dbo].[Students]
    ADD [AvailableDate] DATETIME NULL;
    PRINT 'Column AvailableDate added to Students table.';
END
GO

-- Create StudentSubjects Table (Many-to-Many relationship)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudentSubjects]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StudentSubjects] (
        [StudentId] INT NOT NULL,
        [SubjectId] INT NOT NULL,
        PRIMARY KEY ([StudentId], [SubjectId]),
        FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]) ON DELETE CASCADE,
        FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects]([SubjectId]) ON DELETE CASCADE
    );
END
GO

-- Create AuditLogs Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [AuditLogId] INT IDENTITY(1,1) PRIMARY KEY,
        [Action] NVARCHAR(200) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [IsError] BIT NOT NULL DEFAULT 0
    );
    
    -- Create Index on CreatedDate for faster queries
    CREATE INDEX IX_AuditLogs_CreatedDate ON [dbo].[AuditLogs]([CreatedDate]);
    CREATE INDEX IX_AuditLogs_IsError ON [dbo].[AuditLogs]([IsError]);
END
GO

PRINT 'All tables created successfully!';
GO

