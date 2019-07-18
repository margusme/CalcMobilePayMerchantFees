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
        protected static readonly List<TransactionFeeCalculator> TransactionFeeMerchantCalculators = new List<TransactionFeeCalculator>() { TransactionFeeCalculator.Instance<TransactionFeeCalculatorTelia>(), TransactionFeeCalculator.Instance<TransactionFeeCalculatorCircleK>() };
        protected static readonly TransactionFeeCalculator TypicalTransactionFeeCalculator = TransactionFeeCalculator.Instance();

        /// <summary>
        /// Method returns right transaction fee calculator classified by transaction object or null if transaction object is null
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>Transaction fee calculator or null</returns>
        public static TransactionFeeCalculator GetRightTransactionFeeCalculator(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                return null;
            }

            foreach (var transactionFeeMerchantCalculator in TransactionFeeMerchantCalculators)
            {
                if (transactionFeeMerchantCalculator.GivenMerchantNameIsMyMerchantName(transactionObject.MerchantName))
                {
                    return transactionFeeMerchantCalculator;
                }
            }

            return TypicalTransactionFeeCalculator;
        }

        /// <summary>
        /// Method calculates payment transaction fee for the merchant
        /// </summary>
        /// <param name="transactionObject">Payment transaction object with merchant name</param>
        /// <returns>Payment transaction fee for given merchant.</returns>
        public static decimal CalculateTransactionFeeByMerchant(TransactionObject transactionObject)
        {
            var transactionFeeCalculator = GetRightTransactionFeeCalculator(transactionObject);

            if (transactionFeeCalculator == null)
            {
                return 0;
            }

            return transactionFeeCalculator.CalculateTotalTransactionFee(transactionObject);
        }
    }
}
