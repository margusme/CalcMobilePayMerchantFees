using CalcMobilePayMerchantFees.Models;
using System;

namespace CalcMobilePayMerchantFees.TransactionProcessing.Clients
{
    public class TransactionFeeCalculatorCircleK : TransactionFeeCalculator
    {
        public new static string MerchantName = "CIRCLE_K";

        protected TransactionFeeCalculatorCircleK() : base()
        {

        }

        /// <summary>
        /// Returns payment transaction fee rate for discount. If discount is 20% then it will be 0.8
        /// </summary>
        /// <returns>0.8</returns>
        protected override decimal GetFullTransactionFeeRate()
        {
            return 0.8m;
        }

        /// <summary>
        /// Gets merchant name from static constant
        /// </summary>
        /// <returns></returns>
        protected override string GetMerchantName()
        {
            return MerchantName;
        }
    }
}
