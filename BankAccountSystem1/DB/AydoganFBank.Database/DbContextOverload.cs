﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Database
{
    public partial class AydoganFBankDbContext : DbContext 
    {
        public Guid Guid { get; private set; }
        public AydoganFBankDbContext(string connectionString)
            : base(connectionString)
        {
            Guid = Guid.NewGuid();
        }
    }
}
