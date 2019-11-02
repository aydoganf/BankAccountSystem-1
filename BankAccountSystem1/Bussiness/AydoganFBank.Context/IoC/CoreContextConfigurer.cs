using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.IoC
{
    public class CoreContextConfigurer : ICoreContextConfigurer
    {
        private string connStr = "";
        private string logFileDirectory = "";

        public CoreContextConfigurer(Action<CoreContextConfigurer> action)
        {
            action.Invoke(this);
        }

        string ICoreContextConfigurer.GetConnectionString() => connStr;
        string ICoreContextConfigurer.GetLogFileDirectory() => logFileDirectory;

        public void DBConnectionString(string connStr)
        {
            this.connStr = connStr;
        }

        public void LogFileDirectory(string logFileDirectory)
        {
            this.logFileDirectory = logFileDirectory;
        }
    }

    public class CoreContextConfigurerExpression
    {

    }
}
