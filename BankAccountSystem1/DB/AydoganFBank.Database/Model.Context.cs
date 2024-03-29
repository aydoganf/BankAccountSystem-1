﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AydoganFBankDbContext : DbContext
    {
        public AydoganFBankDbContext()
            : base("name=AydoganFBankDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<TransactionStatus> TransactionStatus { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<CreditCardExtre> CreditCardExtre { get; set; }
        public virtual DbSet<CreditCardExtreDischarge> CreditCardExtreDischarge { get; set; }
        public virtual DbSet<CreditCardPayment> CreditCardPayment { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        public virtual DbSet<TransactionOrder> TransactionOrder { get; set; }
        public virtual DbSet<AccountTransaction> AccountTransaction { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CreditCard> CreditCard { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Token> Token { get; set; }
    }
}
