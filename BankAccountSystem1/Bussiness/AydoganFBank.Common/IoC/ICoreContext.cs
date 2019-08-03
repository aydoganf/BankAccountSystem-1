using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.IoC
{
    public interface ICoreContext
    {
        T New<T>();
    }
}
