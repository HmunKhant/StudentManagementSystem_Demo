# SQL Scripts for Student Management System

This folder contains SQL scripts for setting up the database for the Student Management System.

## Scripts Overview

### 00_CompleteSetup.sql ⭐ **RECOMMENDED FOR FIRST-TIME SETUP**
- Complete database setup script
- Creates database, all tables, indexes, constraints, and seed data
- Includes AvailableDate column
- **Run this script if you want to set up everything in one go**
- Safe to run multiple times (idempotent)

### 01_CreateDatabase.sql
- Creates the StudentDB database
- Run this first if using individual scripts step-by-step

### 02_CreateTables.sql
- Creates all required tables:
  - `Subjects` - Stores available subjects (English, Math, Science)
  - `Students` - Stores student information (includes AvailableDate column)
  - `StudentSubjects` - Many-to-many relationship table
  - `AuditLogs` - Stores system activity logs
- Creates indexes for better performance
- Sets up foreign key constraints for data integrity
- Includes check constraint for Gender (M or F)
- Includes unique constraint on NRIC
- **Automatically adds AvailableDate column if table exists but column is missing**

### 03_SeedData.sql
- Inserts initial data:
  - English
  - Math
  - Science
- Safe to run multiple times (won't create duplicates)

### 04_AddAvailableDateColumn.sql
- **Use this if you already have the database but need to add AvailableDate column**
- Adds AvailableDate column to existing Students table
- Only runs if the column doesn't already exist

## Usage Instructions

### Option 1: Complete Setup (Recommended for First Time) ⭐
1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Open `00_CompleteSetup.sql`
4. Execute the script (F5 or click Execute)
5. Done! Your database is ready to use.

### Option 2: Step-by-Step Setup
If you prefer to run scripts individually:
1. Execute `01_CreateDatabase.sql` - Creates the database
2. Execute `02_CreateTables.sql` - Creates all tables
3. Execute `03_SeedData.sql` - Inserts seed data

### Option 3: Adding AvailableDate to Existing Database
If you already have the database but need to add the AvailableDate column:
1. Execute `04_AddAvailableDateColumn.sql`

## Database Connection

The application uses the following connection string (configured in Web.config):
```xml
<connectionStrings>
    <add name="DefaultConnection"
         connectionString="Server=.;Database=StudentDB;Trusted_Connection=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Important:** 
- `Server=.` means local SQL Server instance
- If your SQL Server is on a different server, change it to `Server=YourServerName`
- If using SQL Server Express, use `Server=.\SQLEXPRESS`
- If using named instance, use `Server=.\InstanceName`

## Data Integrity Features

- **Unique Constraint**: NRIC must be unique (prevents duplicate registrations)
- **Check Constraint**: Gender must be 'M' or 'F'
- **Foreign Keys**: Ensures referential integrity between Students, Subjects, and StudentSubjects
- **Cascade Delete**: Deleting a student will automatically remove associated subject records
- **Indexes**: Created on frequently searched columns:
  - `IX_Students_NRIC` - For NRIC searches
  - `IX_Students_Name` - For Name searches
  - `IX_AuditLogs_CreatedDate` - For log queries
  - `IX_AuditLogs_IsError` - For error log filtering

## Database Schema

### Students Table
- `StudentId` (PK, Identity) - Primary key
- `NRIC` (Required, Unique, Max 20 chars) - National Registration Identity Card
- `Name` (Required, Max 100 chars) - Student name
- `Gender` (Required, M or F) - Gender with check constraint
- `DateOfBirth` (Required) - Date of birth
- `AvailableDate` (Optional) - When student is available

### Subjects Table
- `SubjectId` (PK, Identity) - Primary key
- `SubjectName` (Max 100 chars) - Subject name

### StudentSubjects Table
- `StudentId` (FK, Composite PK) - Foreign key to Students
- `SubjectId` (FK, Composite PK) - Foreign key to Subjects

### AuditLogs Table
- `AuditLogId` (PK, Identity) - Primary key
- `Action` (Max 200 chars) - Action performed
- `Description` (Max) - Detailed description
- `CreatedDate` (Required, Default: GETDATE()) - When the action occurred
- `IsError` (Required, Default: 0) - Whether this is an error log

## Notes

- All scripts are **idempotent** (can be run multiple times safely)
- Scripts check for existing objects before creating them
- The database name is `StudentDB`
- Default schema is `dbo`
- All scripts use `IF EXISTS` and `IF NOT EXISTS` checks to prevent errors

## Troubleshooting

### "Database already exists" message
- This is normal if the database was created previously
- The script will continue and create missing tables/columns

### "Table already exists" message
- This is normal if tables were created previously
- The script will check for missing columns and add them

### "Column already exists" message
- This is normal if the column was added previously
- No action needed

### Connection Errors
- Ensure SQL Server is running
- Check that you have permission to create databases
- Verify the server name in the connection string

### Permission Errors
- Make sure your SQL Server login has `db_owner` or `db_ddladmin` permissions
- Or use a login with `sysadmin` role for initial setup

## Verification

After running the scripts, verify the setup:

```sql
USE StudentDB;
GO

-- Check if all tables exist
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Check Students table structure
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Students'
ORDER BY ORDINAL_POSITION;

-- Check if subjects are seeded
SELECT * FROM Subjects;

-- Check indexes
SELECT i.name AS IndexName, t.name AS TableName
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE i.name IS NOT NULL
ORDER BY t.name, i.name;
```

Expected output:
- 4 tables: AuditLogs, Students, StudentSubjects, Subjects
- Students table should have 6 columns including AvailableDate
- 3 subjects: English, Math, Science
- Multiple indexes on Students and AuditLogs tables
