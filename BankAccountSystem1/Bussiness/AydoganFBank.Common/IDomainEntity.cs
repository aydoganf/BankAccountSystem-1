using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common
{
    public interface IDomainEntity
    {
        int Id { get; }

        /// <summary>
        /// Inserts the domain entity to db
        /// </summary>
        /// <param name="forceToInsertDb">if it is true, entity will be inserted immediately</param>
        void Insert(bool forceToInsertDb = true);

        /// <summary>
        /// Updates the domain entity
        /// </summary>
        void Save();
    }
}
