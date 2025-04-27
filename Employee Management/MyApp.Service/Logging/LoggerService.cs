using MyApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Service.Logging
{
    /// <summary>
    /// Logger implementation for system and audit logs.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        public void LogInfo(string action, string message)
        {
            Console.WriteLine($"[INFO] {DateTime.UtcNow}: {action} - {message}");
        }

        public void LogAudit(string actionType, string entityName, string entityId)
        {
            Console.WriteLine($"[AUDIT] {DateTime.UtcNow}: {actionType} {entityName} with ID {entityId}");
        }
    }
}
