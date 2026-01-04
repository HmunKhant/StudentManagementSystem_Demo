-- =============================================
-- Script: Complete Database Setup
-- Description: Complete setup script that runs all database creation and seeding
-- Usage: Run this script to set up the entire database from scratch
-- =============================================

-- Step 1: Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'StudentDB')
BEGIN
    CREATE DATABASE StudentDB;
    PRINT 'Database StudentDB created successfully!';
END
ELSE
BEGIN
    PRINT 'Database StudentDB already exists.';
END
GO

USE StudentDB;
GO

-- Step 2: Create Tables
-- Create Subjects Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subjects]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Subjects] (
        [SubjectId] INT IDENTITY(1,1) PRIMARY KEY,
        [SubjectName] NVARCHAR(100) NOT NULL
    );
    PRINT 'Table Subjects created successfully!';
END
ELSE
BEGIN
    PRINT 'Table Subjects already exists.';
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
    PRINT 'Table Students created successfully!';
END
ELSE
BEGIN
    PRINT 'Table Students already exists.';
    
    -- Add AvailableDate column if table exists but column doesn't
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = 'AvailableDate')
    BEGIN
        ALTER TABLE [dbo].[Students]
        ADD [AvailableDate] DATETIME NULL;
        PRINT 'Column AvailableDate added to Students table.';
    END
    ELSE
    BEGIN
        PRINT 'Column AvailableDate already exists in Students table.';
    END
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
    PRINT 'Table StudentSubjects created successfully!';
END
ELSE
BEGIN
    PRINT 'Table StudentSubjects already exists.';
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
    PRINT 'Table AuditLogs created successfully!';
END
ELSE
BEGIN
    PRINT 'Table AuditLogs already exists.';
END
GO

-- Step 3: Seed Data
-- Insert Subjects if they don't exist
IF NOT EXISTS (SELECT * FROM [dbo].[Subjects] WHERE [SubjectName] = 'English')
BEGIN
    INSERT INTO [dbo].[Subjects] ([SubjectName]) VALUES ('English');
    PRINT 'Subject English inserted.';
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Subjects] WHERE [SubjectName] = 'Math')
BEGIN
    INSERT INTO [dbo].[Subjects] ([SubjectName]) VALUES ('Math');
    PRINT 'Subject Math inserted.';
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Subjects] WHERE [SubjectName] = 'Science')
BEGIN
    INSERT INTO [dbo].[Subjects] ([SubjectName]) VALUES ('Science');
    PRINT 'Subject Science inserted.';
END
GO

PRINT '========================================';
PRINT 'Database setup completed successfully!';
PRINT '========================================';
GO

