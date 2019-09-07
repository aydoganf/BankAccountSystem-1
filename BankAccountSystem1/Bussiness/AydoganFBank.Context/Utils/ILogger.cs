using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.Utils
{
    public interface ILogger
    {
        void Error(System.Exception ex);
        void Error(string message);

        void Warning(string message);
        void Warning(string message, object obj);

        void Info(string message);
        void Info(string message, object obj);
    }
}
