using CalcMobilePayMerchantFees.Models;
using System;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    public class TransactionFeeCalculatorTelia : TransactionFeeCalculator
    {
        public new static string MerchantName = "TELIA";

        /// <summary>
        /// Returns payment transaction fee
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>0.00 at the moment</returns>
        public new static decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            var baseTransactionFee = TransactionFeeCalculator.CalculateTransactionFee(transactionObject);

            return Math.Round(baseTransactionFee * (decimal)0.9, 2);
        }
    }
}
