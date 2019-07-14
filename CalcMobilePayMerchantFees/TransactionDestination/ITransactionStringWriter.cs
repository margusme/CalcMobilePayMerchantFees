using System;
using System.Collections.Generic;
using System.Text;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionDestination
{
    /// <summary>
    /// Used for classes that deals with writing payment transaction data from object into string
    /// </summary>
    public interface ITransactionStringWriter
    {
        string WriteTransactionData(TransactionObject transactionObject);
    }
}
