-- =============================================
-- Script: Add AvailableDate Column to Students Table
-- Description: Adds the AvailableDate column to existing Students table
-- Usage: Run this if you already have the Students table but need to add AvailableDate
-- =============================================

USE StudentDB;
GO

-- Check if Students table exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
BEGIN
    -- Check if AvailableDate column already exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND name = 'AvailableDate')
    BEGIN
        ALTER TABLE [dbo].[Students]
        ADD [AvailableDate] DATETIME NULL;
        
        PRINT 'Column AvailableDate added successfully to Students table.';
    END
    ELSE
    BEGIN
        PRINT 'Column AvailableDate already exists in Students table.';
    END
END
ELSE
BEGIN
    PRINT 'ERROR: Students table does not exist. Please run 02_CreateTables.sql first.';
END
GO

