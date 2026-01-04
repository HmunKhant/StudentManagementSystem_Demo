using StudentManagementSystem.Data;
using StudentManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ApplicationDbContext _context;

        public LoggingService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Log(string action, string description, bool isError = false)
        {
            try
            {
                _context.AuditLogs.Add(new AuditLog
                {
                    Action = action,
                    Description = description,
                    CreatedDate = DateTime.Now,
                    IsError = isError
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log error to file or event log if database logging fails
                System.Diagnostics.Debug.WriteLine($"Logging Error: {ex.Message}");
            }
        }
    }

}