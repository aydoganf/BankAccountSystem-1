using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aydoganfbank.web.api2.Utility
{
    public static class Extensions
    {
        public static T ParseToEnum<T>(this int val)
        {
            return (T)Enum.Parse(typeof(T), val.ToString());
        }
    }
}
