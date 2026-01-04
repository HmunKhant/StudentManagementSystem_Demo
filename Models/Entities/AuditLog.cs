using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models.Entities
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsError { get; set; }
    }
}