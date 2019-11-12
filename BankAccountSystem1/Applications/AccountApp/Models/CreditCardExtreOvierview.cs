using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AydoganFBank.Service.Dispatcher.Data;

namespace AccountApp.Models
{
    public class CreditCardExtreOvierview
    {
        public CreditCardInfo CreditCard { get; set; }
        public List<CreditCardExtreOverviewItem> ExtreDetailList { get; set; }

        public CreditCardExtreOvierview(CreditCardInfo creditCard, List<CreditCardExtreInfo> creditCardExtres, List<CreditCardPaymentInfo> creditCardPayments)
        {
            CreditCard = creditCard;
            ExtreDetailList = new List<CreditCardExtreOverviewItem>();

            foreach (var extre in creditCardExtres)
            {
                CreditCardExtreOverviewItem overviewItem = new CreditCardExtreOverviewItem(extre);
                overviewItem.SetPaymentList(
                    creditCardPayments.Where(p => p.InstalmentDate >= extre.ExtreStartDate && p.InstalmentDate <= extre.ExtreEndDate).
                    ToList());

                ExtreDetailList.Add(overviewItem);
            }
        }
    }
}