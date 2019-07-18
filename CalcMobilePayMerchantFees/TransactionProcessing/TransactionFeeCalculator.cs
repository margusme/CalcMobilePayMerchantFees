using System;
using System.Collections.Generic;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    /// <summary>
    /// Calculates payment transaction fee for general transaction types
    /// </summary>
    public class TransactionFeeCalculator
    {
        public static string MerchantName = "";
        public static decimal InvoiceFixedFee = 29.00m;

        protected static HashSet<string> ClientMonthFirstOperations = new HashSet<string>();

        private static readonly Dictionary<string, TransactionFeeCalculator> Instances = new Dictionary<string, TransactionFeeCalculator>();

        protected TransactionFeeCalculator()
        {

        }

        /// <summary>
        /// Gets instance of the current class
        /// </summary>
        /// <returns></returns>
        public static TransactionFeeCalculator Instance()
        {
            return GetInstance<TransactionFeeCalculator>();
        }

        /// <summary>
        /// Gets instance whether the current or any child classes
        /// </summary>
        /// <typeparam name="T">Type of the class</typeparam>
        /// <returns>new instance of T</returns>
        public static TransactionFeeCalculator Instance<T>() where T : TransactionFeeCalculator
        {
            return GetInstance<T>();
        }

        /// <summary>
        /// Returns payment transaction fee with additional charges for the first merchant transaction in month. 
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of the transaction amount plus additional charges</returns>
        public virtual decimal CalculateTotalTransactionFee(TransactionObject transactionObject)
        {
            return CalculateTransactionFee(transactionObject) + CalculateAdditionalFirstDayFee(transactionObject);
        }

        /// <summary>
        /// Validates if given merchant name equals to calculator's merchant name
        /// </summary>
        /// <param name="merchantName"></param>
        /// <returns></returns>
        public bool GivenMerchantNameIsMyMerchantName(string merchantName)
        {
            return GetMerchantName().Trim().ToUpper().Equals(merchantName.Trim().ToUpper());
        }

        /// <summary>
        /// Gets instance whether the current or any child classes
        /// </summary>
        /// <typeparam name="T">Type of the class</typeparam>
        /// <returns>new instance of T</returns>
        protected static TransactionFeeCalculator GetInstance<T>() where T : TransactionFeeCalculator
        {
            var type = typeof(T);

            if (!Instances.ContainsKey(type.FullName))
            {
                Instances.TryAdd(type.FullName, (TransactionFeeCalculator)Activator.CreateInstance(type, true));
            }

            return Instances[type.FullName];
        }

        /// <summary>
        /// Gets merchant name. Empty for general/common payment transaction
        /// </summary>
        /// <returns>Empty</returns>
        protected virtual string GetMerchantName()
        {
            return MerchantName;
        }

        /// <summary>
        /// Returns payment transaction fee. 
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>1/100 of the transaction amount multiplied by rate. If rate is 0.9 then it will give 10% of discount</returns>
        protected virtual decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                return 0.00m;
            }

            return Math.Round(transactionObject.TransactionAmount * GetFullTransactionFeeRate() / 100, 2);
        }

        /// <summary>
        /// Calculates additional fee for the merchant according to the formula:
        /// - Invoice Fee should be included in the fee for first transaction of the month
        /// - If there aren't any transactions that month, Merchant should not be charged Invoice Fee
        /// - If transaction fee is 0 after applying discounts, InvoiceFee should not be added
        /// If merchant has been charged then returns 0.00
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>Additional fee</returns>
        protected virtual decimal CalculateAdditionalFirstDayFee(TransactionObject transactionObject)
        {
            return !MerchantHasTransactionsForMonth(transactionObject) && CalculateTransactionFee(transactionObject) > 0 ? InvoiceFixedFee : 0.00m;
        }

        /// <summary>
        /// Returns payment transaction fee rate for discount. If discount is 0% then it will be 1.0
        /// </summary>
        /// <returns>1.0</returns>
        protected virtual decimal GetFullTransactionFeeRate()
        {
            return 1.00m;
        }

        /// <summary>
        /// Returns true if merchant has already some transactions in the month given by payment transaction object.
        /// If merchant has not been charged then adds merchant data to the month given by payment transaction object.
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>true if merchant has already some transactions in the month given by payment transaction object</returns>
        protected static bool MerchantHasTransactionsForMonth(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                //Do not add fee if have no data 
                return true;
            }

            var keyForFirstRecordInMonth = transactionObject.GetKeyForFirstRecordInMonth;
            var clientHasTransactionsForMonth = ClientMonthFirstOperations.Contains(keyForFirstRecordInMonth);

            if (!clientHasTransactionsForMonth)
            {
                ClientMonthFirstOperations.Add(keyForFirstRecordInMonth);
            }

            return clientHasTransactionsForMonth;
        }
    }
}
