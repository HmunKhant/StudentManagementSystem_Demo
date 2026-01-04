using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StudentManagementSystem.Data;
using StudentManagementSystem.Migrations;

namespace StudentManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
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
        }
    }
}
