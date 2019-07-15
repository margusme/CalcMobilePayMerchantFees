using System.Collections;
using System.Collections.Generic;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionProcessing.Clients;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    /// <summary>
    /// Class is used to classify payment transaction by merchant type
    /// </summary>
    public class TransactionMerchantClassifier
    {
        /// <summary>
        /// Hold client-specific calculator objects
        /// </summary>
        protected static readonly List<TransactionFeeCalculator> TransactionFeeClientCalculators = new List<TransactionFeeCalculator>() { new TransactionFeeCalculatorTelia(), new TransactionFeeCalculatorCircleK() };
        protected static readonly TransactionFeeCalculator TypicalTransactionFeeCalculator = new TransactionFeeCalculator();

        protected static Hashtable ClientMonthFirstOperations = new Hashtable();

        /// <summary>
        /// Method calculates payment transaction fee for the merchant
        /// </summary>
        /// <param name="transactionObject">Payment transaction object with merchant name</param>
        /// <returns>Payment transaction fee for given merchant.</returns>
        public static decimal CalculateTransactionFeeByMerchant(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                return 0;
            }

            foreach (var transactionFeeClientCalculator in TransactionFeeClientCalculators)
            {
                if (transactionObject.MerchantName.ToUpper().Equals(transactionFeeClientCalculator.GetMerchantName()))
                {
                    return transactionFeeClientCalculator.CalculateTotalTransactionFee(transactionObject);
                }
            }

            return TypicalTransactionFeeCalculator.CalculateTotalTransactionFee(transactionObject);
        }
    }
}
