using CalcMobilePayMerchantFees.Models;
using System;

namespace CalcMobilePayMerchantFees.TransactionProcessing.Clients
{
    public class TransactionFeeCalculatorTelia : TransactionFeeCalculator
    {
        public new static string MerchantName = "TELIA";

        protected TransactionFeeCalculatorTelia() : base()
        {

        }

        /// <summary>
        /// Returns payment transaction fee rate for discount. If discount is 10% then it will be 0.9
        /// </summary>
        /// <returns>0.9</returns>
        protected override decimal GetFullTransactionFeeRate()
        {
            return 0.9m;
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
