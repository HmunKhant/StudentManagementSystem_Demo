# Student Management System

A simple web application built with ASP.NET MVC 5 for managing student registrations.

## Features

### Part 1 - Registration Page
- **NRIC** (required) - Unique identifier for each student
- **Name** (required)
- **Gender** (required) - Male (M) or Female (F)
- **Birthday** (required) - Date of birth
- **Available Date** (optional) - When the student is available
- **Subjects** (optional) - Multiple selection from:
  - English
  - Math
  - Science

### Part 2 - View All Users Page
- Displays all registered students with:
  - Serial Number (S/N)
  - NRIC (clickable link to edit)
  - Name
  - Gender (M or F)
  - Age (calculated from date of birth)
  - Number of Subjects
- **Search functionality** - Filter by NRIC or Name
- **Edit functionality** - Click on NRIC to edit student details

### Additional Features
- **Comprehensive logging** - All user actions are logged in the AuditLogs table
- **Error handling** - Errors are caught and logged
- **Data integrity** - Unique constraints, foreign keys, and validation
- **User-friendly UI** - Bootstrap-styled forms and tables

## Requirements

- .NET Framework 4.8
- ASP.NET MVC 5
- SQL Server (LocalDB or SQL Server Express/Standard)
- Visual Studio 2019 or later (for development)

## Setup Instructions

### 1. Database Setup

You have two options for setting up the database:

#### Option A: Using SQL Scripts (Recommended for first-time setup)

1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Open and execute `SQL/00_CompleteSetup.sql`
   - This will create the database, all tables, indexes, and seed data

#### Option B: Using Entity Framework Migrations

1. Open Package Manager Console in Visual Studio
2. Run the following commands:
   ```
   Update-Database
   ```
   This will create the database and apply all migrations, including seeding the Subjects table.

### 2. Configure Connection String

The connection string is already configured in `Web.config`:
```xml
<connectionStrings>
    <add name="DefaultConnection"
         connectionString="Server=.;Database=StudentDB;Trusted_Connection=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

If your SQL Server instance is different, update the connection string accordingly.

### 3. Run the Application

1. Build the solution (Ctrl+Shift+B)
2. Press F5 to run the application
3. The application will open in your default browser
4. Navigate to the Students page to start registering students

## Project Structure

```
StudentManagementSystem/
├── Controllers/
│   ├── HomeController.cs
│   └── StudentController.cs       # Main controller for student operations
├── Models/
│   ├── Entities/                  # Database entities
│   │   ├── Student.cs
│   │   ├── Subject.cs
│   │   ├── StudentSubject.cs
│   │   └── AuditLog.cs
│   └── ViewModels/                # View models
│       ├── StudentCreateVM.cs
│       ├── StudentEditVM.cs
│       └── StudentListVM.cs
├── Views/
│   ├── Student/
│   │   ├── Create.cshtml          # Registration form
│   │   ├── Edit.cshtml            # Edit student form
│   │   └── Index.cshtml          # List all students
│   └── Shared/
│       └── _Layout.cshtml
├── Data/
│   └── ApplicationDbContext.cs    # Entity Framework context
├── Services/
│   └── LoggingService.cs         # Service for logging actions
├── Migrations/                    # Entity Framework migrations
├── SQL/                           # SQL scripts for database setup
│   ├── 00_CompleteSetup.sql
│   ├── 01_CreateDatabase.sql
│   ├── 02_CreateTables.sql
│   ├── 03_SeedData.sql
│   └── README.md
└── Web.config                     # Application configuration
```

## Database Schema

### Students Table
- `StudentId` (PK, Identity)
- `NRIC` (Unique, Required)
- `Name` (Required)
- `Gender` (Required, M or F)
- `DateOfBirth` (Required)
- `AvailableDate` (Optional)

### Subjects Table
- `SubjectId` (PK, Identity)
- `SubjectName`

### StudentSubjects Table
- `StudentId` (FK, Composite PK)
- `SubjectId` (FK, Composite PK)

### AuditLogs Table
- `AuditLogId` (PK, Identity)
- `Action`
- `Description`
- `CreatedDate`
- `IsError`

## Usage

### Registering a Student

1. Click on "Students" in the navigation menu or go to the home page
2. Click "Register New Student"
3. Fill in the required fields:
   - NRIC (must be unique)
   - Name
   - Gender
   - Birthday
4. Optionally fill in:
   - Available Date
   - Select one or more subjects (hold Ctrl/Cmd to select multiple)
5. Click "Submit"

### Viewing All Students

1. Navigate to the Students page
2. View the list of all registered students
3. Use the search box to filter by NRIC or Name
4. Click on an NRIC to edit that student's information

### Editing a Student

1. From the student list, click on the NRIC of the student you want to edit
2. Modify any fields as needed
3. Click "Update" to save changes

## Data Integrity Features

- **Unique NRIC**: Each student must have a unique NRIC
- **Foreign Key Constraints**: Ensures referential integrity
- **Cascade Delete**: Deleting a student removes associated subject records
- **Validation**: Client-side and server-side validation
- **Error Logging**: All errors are logged to the AuditLogs table

## Logging

All user actions are automatically logged:
- Viewing student list
- Creating a new student
- Editing a student
- Any errors that occur

Logs can be viewed in the `AuditLogs` table in the database.

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server is running
- Verify the connection string in `Web.config`
- Check that the database `StudentDB` exists

### Migration Issues
- If using migrations, ensure you have run `Update-Database`
- Check the `Migrations` folder for migration files

### Subject Selection Not Working
- Ensure the Subjects table has been seeded with English, Math, and Science
- Run the seed script or migration

## Notes

- The application uses Entity Framework Code First approach
- All dates are stored in the database as DATETIME
- Age is calculated dynamically from DateOfBirth
- The default route is set to Student/Index

## License

This project is provided as-is for educational purposes.

