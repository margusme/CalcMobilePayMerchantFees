using System;
using System.Collections.Generic;
using System.Text;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionSource
{
    /// <summary>
    /// Interface for reading payment transactions data from any kind of string sources
    /// </summary>
    public interface ITransactionStringReader
    {
        /// <summary>
        /// Gets payment transaction object from string data
        /// </summary>
        /// <param name="transactionData">String with transaction data</param>
        /// <returns>Payment transaction object</returns>
        TransactionObject ReadTransaction(string transactionData);
    }
}
