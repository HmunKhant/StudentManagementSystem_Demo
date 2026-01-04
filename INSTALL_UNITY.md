# Installing Unity Container for Dependency Injection

To complete the dependency injection setup, you need to install Unity Container packages via NuGet.

## Installation Steps

### Option 1: Using Package Manager Console (Recommended)

1. Open Visual Studio
2. Go to **Tools** → **NuGet Package Manager** → **Package Manager Console**
3. Run the following commands:

```powershell
Install-Package Unity.Mvc5
```

This will automatically install:
- Unity (core library)
- Unity.Mvc5 (MVC 5 integration)

### Option 2: Using NuGet Package Manager UI

1. Right-click on your project in Solution Explorer
2. Select **Manage NuGet Packages...**
3. Click on **Browse** tab
4. Search for **Unity.Mvc5**
5. Click **Install**

## Verify Installation

After installation, check that the following packages are in your `packages.config`:

```xml
<package id="Unity" version="5.11.11" targetFramework="net48" />
<package id="Unity.Mvc5" version="5.11.1" targetFramework="net48" />
```

## What Was Created

The following files have been created for dependency injection:

1. **Data/IStudentRepository.cs** - Repository interface
2. **Data/StudentRepository.cs** - Repository implementation
3. **Services/IStudentService.cs** - Service interface
4. **Services/StudentService.cs** - Service implementation
5. **Services/ILoggingService.cs** - Logging service interface (updated)
6. **App_Start/UnityConfig.cs** - Unity DI configuration
7. **Controllers/StudentController.cs** - Updated to use constructor injection

## Architecture

```
Controller → Service → Repository → DbContext
```

- **Controller**: Handles HTTP requests/responses
- **Service**: Contains business logic
- **Repository**: Handles data access
- **DbContext**: Entity Framework context

## Benefits

- **Separation of Concerns**: Each layer has a single responsibility
- **Testability**: Easy to mock dependencies for unit testing
- **Maintainability**: Changes in one layer don't affect others
- **Flexibility**: Easy to swap implementations

## After Installation

Once Unity is installed, the application will automatically:
1. Register all dependencies in `UnityConfig.cs`
2. Inject dependencies into controllers via constructor
3. Manage object lifetimes (DbContext per request)

No additional configuration needed - just build and run!

