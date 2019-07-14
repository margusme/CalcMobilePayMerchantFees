using CalcMobilePayMerchantFees.Models;
using System;

namespace CalcMobilePayMerchantFees.TransactionProcessing.Clients
{
    public class TransactionFeeCalculatorCircleK : TransactionFeeCalculator
    {
        public new static string MerchantName = "CIRCLE_K";

        /// <summary>
        /// Returns payment transaction fee. Non-static method
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of transaction amount minus 20%</returns>
        public override decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            var baseTransactionFee = base.CalculateTransactionFee(transactionObject);

            return Math.Round(baseTransactionFee * 0.8m, 2);
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
