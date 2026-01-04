using StudentManagementSystem.Data;
using StudentManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

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

                // Also log to Serilog file
                //if (isError)
                //{
                //    Log.Error("Action: {Action} | Description: {Description}", action, description);
                //}
                //else
                //{
                //    Log.Information("Action: {Action} | Description: {Description}", action, description);
                //}
            }
            catch (Exception ex)
            {
                // Log error to Serilog file if database logging fails
                //Log.Error(ex, "Failed to log to database. Action: {Action} | Description: {Description}", action, description);
            }
        }
    }

}