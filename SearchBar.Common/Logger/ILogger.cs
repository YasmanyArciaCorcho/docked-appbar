using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public interface ILogger
    {
        void Trace(string message);

        void Trace(string format, params object[] args);

        void Info(string message);

        void Error(string message);

        void Error(string message, Exception exception);

        void Error(Exception exception);
    }
}
