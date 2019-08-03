using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common
{
    public interface IDomainEntity
    {
        int Id { get; }

        void Insert(bool forceToınsertDb = true);
        void Save();
    }
}
