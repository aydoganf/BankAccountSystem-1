using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.IoC
{
    public class CoreContextConfigurer : ICoreContextConfigurer
    {
        private string ConnStr = "";

        public CoreContextConfigurer(Action<CoreContextConfigurer> action)
        {
            action.Invoke(this);
        }

        string ICoreContextConfigurer.GetConnectionString()
        {
            return ConnStr;
        }

        public void SetConnStr(string connStr)
        {
            this.ConnStr = connStr;
        }
    }

    public class CoreContextConfigurerExpression
    {

    }
}
