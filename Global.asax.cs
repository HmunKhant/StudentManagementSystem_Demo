using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StudentManagementSystem.Data;
using StudentManagementSystem.Migrations;
using Serilog;
using Serilog.Events;

namespace StudentManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Configure Serilog for file logging
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log-.txt");
            var logDirectory = Path.GetDirectoryName(logPath);
            
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    fileSizeLimitBytes: 10485760) // 10 MB per file
                .CreateLogger();

            Log.Information("Application starting...");

            AreaRegistration.RegisterAllAreas();
            
            // Unity DI Container is registered via UnityMvcActivator (runs before Application_Start)
            // UnityConfig.RegisterComponents() is called in UnityMvcActivator.Start()
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Run database migrations automatically
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            
            //// Ensure database is created and migrations are applied
            //using (var context = new ApplicationDbContext())
            //{
            //    var migrator = new DbMigrator(new Configuration());
            //    migrator.Update();
            //}

            Log.Information("Application started successfully");
        }

        protected void Application_End()
        {
            Log.Information("Application shutting down...");
            Log.CloseAndFlush();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Log.Error(exception, "Unhandled application error");
        }
    }
}
