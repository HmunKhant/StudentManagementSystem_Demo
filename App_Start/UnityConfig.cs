using StudentManagementSystem.Data;
using StudentManagementSystem.Services;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace StudentManagementSystem
{
    public static class UnityConfig
    {
        public static IUnityContainer Container { get; private set; }

        public static void RegisterComponents()
        {
            Container = new UnityContainer();

            // Register DbContext with PerRequestLifetimeManager (one per HTTP request)
            Container.RegisterType<ApplicationDbContext>(new PerRequestLifetimeManager());

            // Register Repositories
            Container.RegisterType<IStudentRepository, StudentRepository>();

            // Register Services
            Container.RegisterType<ILoggingService, LoggingService>();
            Container.RegisterType<IStudentService, StudentService>();

            // Set MVC dependency resolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}

