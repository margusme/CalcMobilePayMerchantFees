using CalcMobilePayMerchantFees.Models;
using System;

namespace CalcMobilePayMerchantFees.TransactionProcessing.Clients
{
    public class TransactionFeeCalculatorTelia : TransactionFeeCalculator
    {
        public new static string MerchantName = "TELIA";

        /// <summary>
        /// Returns payment transaction fee. Non-static method
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of transaction amount minus 10%</returns>
        public override decimal CalculateTransactionObjectFee(TransactionObject transactionObject)
        {
            var baseTransactionFee = base.CalculateTransactionObjectFee(transactionObject);

            return Math.Round(baseTransactionFee * (decimal)0.9, 2);
        }

        /// <summary>
        /// Gets merchant name from static constant
        /// </summary>
        /// <returns></returns>
        public override string GetMerchantName()
        {
            return MerchantName;
        }
    }
}
