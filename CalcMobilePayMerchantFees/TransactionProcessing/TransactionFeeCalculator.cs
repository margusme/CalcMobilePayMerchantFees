using System;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    /// <summary>
    /// Calculates payment transaction fee for general transaction types
    /// </summary>
    public class TransactionFeeCalculator
    {
        public static string MerchantName = "";

        /// <summary>
        /// Returns payment transaction fee. Static method
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of the transaction amount</returns>
        public static decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                return (decimal) 0.00;
            }

            return Math.Round((decimal)transactionObject.TransactionAmount / 100, 2);
        }

        /// <summary>
        /// Returns payment transaction fee. Non-static method
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of the transaction amount</returns>
        public virtual decimal CalculateTransactionObjectFee(TransactionObject transactionObject)
        {
            return CalculateTransactionFee(transactionObject);
        }

        public virtual string GetMerchantName()
        {
            return MerchantName;
        }
    }
}
