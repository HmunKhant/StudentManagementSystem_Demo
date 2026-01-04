-- =============================================
-- Script: Create Database
-- Description: Creates the StudentDB database
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'StudentDB')
BEGIN
    CREATE DATABASE StudentDB;
END
GO

USE StudentDB;
GO

