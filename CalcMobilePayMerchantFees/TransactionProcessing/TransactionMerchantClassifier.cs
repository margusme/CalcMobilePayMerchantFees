using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    /// <summary>
    /// Class is used to classify payment transaction by merchant type
    /// </summary>
    public class TransactionMerchantClassifier
    {
        /// <summary>
        /// Method calculates payment transaction fee for the merchant
        /// </summary>
        /// <param name="transactionObject">Payment transaction object with merchant name</param>
        /// <returns>Payment transaction fee for given merchant. At the moment returns the result for all payment transactions the same way</returns>
        public static decimal CalculateTransactionFeeByMerchant(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                return 0;
            }

            if (transactionObject.MerchantName.ToUpper().Equals(TransactionFeeCalculatorTelia.MerchantName))
            {
                return TransactionFeeCalculatorTelia.CalculateTransactionFee(transactionObject);
            }

            return TransactionFeeCalculator.CalculateTransactionFee(transactionObject);
        }
    }
}
