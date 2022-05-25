using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public class NLogger : ILogger
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public void Trace(string message)
        {
            Logger.Info(message);
        }

        public void Trace(string format, params object[] args)
        {
            Logger.Info(format, args);
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            Logger.Error(exception, message);
        }

        public void Error(Exception exception)
        {
            Logger.Error(exception);
        }
    }

    public static class StaticLogger
    {
        static NLogger _logger;
        public static NLogger Logger
        {
            get
            {
                if (_logger == null)
                {
#if DEBUG
                    var config = new NLog.Config.LoggingConfiguration();
                    var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
                    //var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

                    config.AddRule(LogLevel.Debug, LogLevel.Trace, logfile);
                    config.AddRule(LogLevel.Debug, LogLevel.Error, logfile);
                    NLog.LogManager.Configuration = config;
#else

#endif
                    _logger = new NLogger();
                }
                return _logger;
            }
        }
    }
}
