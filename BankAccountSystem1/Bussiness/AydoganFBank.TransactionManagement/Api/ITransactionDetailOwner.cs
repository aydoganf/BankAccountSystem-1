﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.TransactionManagement.Api
{
    public interface ITransactionDetailOwner
    {
        int OwnerId { get; }
        TransactionDetailOwnerType OwnerType { get; }
    }
}