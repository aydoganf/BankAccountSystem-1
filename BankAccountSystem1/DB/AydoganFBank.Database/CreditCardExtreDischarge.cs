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
    
    public partial class CreditCardExtreDischarge
    {
        public int CreditCardExtreDischargeId { get; set; }
        public decimal DischargeAmount { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreditCardExtreId { get; set; }
        public string AccountTransactionId { get; set; }
    }
}
