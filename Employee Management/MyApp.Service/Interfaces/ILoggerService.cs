using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Service.Interfaces
{
    /// <summary>
    /// Interface for logging (normal + audit)
    /// </summary>
    public interface ILoggerService
    {
        void LogInfo(string action, string message);
        void LogAudit(string actionType, string entityName, string entityId);
    }
}
