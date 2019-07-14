using System;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    /// <summary>
    /// Calculates payment transaction fee for general transaction types
    /// </summary>
    public class TransactionFeeCalculator
    {
        /// <summary>
        /// Returns payment transaction fee
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>0.00 at the moment</returns>
        public static decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            return (decimal) 0.00;
        }
    }
}
