namespace StudentManagementSystem.Services
{
    public interface ILoggingService
    {
        void Log(string action, string description, bool isError = false);
    }
}

