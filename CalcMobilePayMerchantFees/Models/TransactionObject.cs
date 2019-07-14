using System;
using System.Collections.Generic;
using System.Text;

namespace CalcMobilePayMerchantFees.Models
{
    /// <summary>
    /// Object keeps properties for transaction data both for incoming and outgoing sources
    /// </summary>
    public class TransactionObject
    {
        public DateTime TransactionDate { get; set; }
        public string MerchantName { get; set; }
        public long TransactionAmount { get; set; }
        public decimal TransactionPercentageFee { get; set; }
        public string GetKeyForFirstRecordInMonth
        {
            get
            {
                return MerchantName.ToUpper() + "_" +
                       TransactionDate.Year + "_" +
                       TransactionDate.Month;
            }
        }
    }
}
