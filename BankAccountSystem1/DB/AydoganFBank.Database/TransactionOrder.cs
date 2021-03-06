//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AydoganFBank.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionOrder
    {
        public int TransactionOrderId { get; set; }
        public int TransactionTypeId { get; set; }
        public string OrderDescription { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime OperationDate { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public int TransactionOrderStatusId { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}
