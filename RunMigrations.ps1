# PowerShell script to run Entity Framework 6 migrations
# This script uses the Entity Framework Migrate.exe tool

Write-Host "Running Entity Framework Migrations..." -ForegroundColor Green

# Get the path to Migrate.exe (usually in packages folder)
$migrateExePath = "packages\EntityFramework.6.5.1\tools\migrate.exe"

if (Test-Path $migrateExePath) {
    Write-Host "Found Migrate.exe at: $migrateExePath" -ForegroundColor Green
    
    # Run the migration
    & $migrateExePath "StudentManagementSystem.dll" /startupConfigurationFile="Web.config" /connectionStringName="DefaultConnection" /connectionProviderName="System.Data.SqlClient"
    
    Write-Host "Migration completed!" -ForegroundColor Green
} else {
    Write-Host "Migrate.exe not found. Please run migrations using one of the following methods:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Method 1: Using Package Manager Console in Visual Studio" -ForegroundColor Cyan
    Write-Host "  1. Open Visual Studio" -ForegroundColor White
    Write-Host "  2. Go to Tools > NuGet Package Manager > Package Manager Console" -ForegroundColor White
    Write-Host "  3. Run: Update-Database" -ForegroundColor White
    Write-Host ""
    Write-Host "Method 2: Using SQL Scripts" -ForegroundColor Cyan
    Write-Host "  1. Open SQL Server Management Studio" -ForegroundColor White
    Write-Host "  2. Execute SQL\00_CompleteSetup.sql" -ForegroundColor White
    Write-Host ""
}

