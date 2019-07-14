using System;
using System.Collections.Generic;
using System.Text;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionDestination
{
    /// <summary>
    /// Used for classes that will write all payment transactions into file, console etc.
    /// </summary>
    public interface ITransactionsWriter
    {
        bool Open();
        bool WriteNextTransaction(TransactionObject transactionObject);
        bool HasError();
        void Close();
    }
}
