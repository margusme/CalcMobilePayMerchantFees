using CalcMobilePayMerchantFees.Models;
using System;

namespace CalcMobilePayMerchantFees.TransactionProcessing.Clients
{
    public class TransactionFeeCalculatorTelia : TransactionFeeCalculator
    {
        public new static string MerchantName = "TELIA";

        /// <summary>
        /// Returns payment transaction fee. 
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of transaction amount minus 10%</returns>
        public override decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            var baseTransactionFee = base.CalculateTransactionFee(transactionObject);

            return Math.Round(baseTransactionFee * 0.9m, 2);
        }

        /// <summary>
        /// Returns payment transaction fee with additional charges for the first merchant transaction in month. 
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of the transaction amount minus 10% plus additional charges</returns>
        public override decimal CalculateTotalTransactionFee(TransactionObject transactionObject)
        {
            return CalculateTransactionFee(transactionObject) + CalculateAdditionalFirstDayFee(transactionObject);
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
