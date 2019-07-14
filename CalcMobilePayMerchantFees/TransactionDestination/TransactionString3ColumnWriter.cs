using System;
using System.Globalization;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionDestination
{
    /// <summary>
    /// Used to write payment transaction data from object into 3 column string line
    /// </summary>
    public class TransactionString3ColumnWriter : ITransactionStringWriter
    {
        private readonly ICalcFeesLogger _logger;

        protected virtual string Separator => " ";
        protected virtual string OutgoingDateFormat => "yyyy-MM-dd";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public TransactionString3ColumnWriter(ICalcFeesLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
        }

        /// <summary>
        /// Method to actually write payment transaction data into string
        /// </summary>
        /// <param name="transactionObject">Payment transaction object</param>
        /// <returns>String with transaction date, merchant name and percentage fee</returns>
        public string WriteTransactionData(TransactionObject transactionObject)
        {
            if (transactionObject == null)
            {
                return "";
            }

            try
            {
                return transactionObject.TransactionDate.ToString(OutgoingDateFormat) + Separator + transactionObject.MerchantName + Separator + transactionObject.TransactionPercentageFee.ToString("0.00");
            }
            catch (Exception e)
            {
                _logger.WriteLine("Could not read next string line from transaction object, exception: " + e.ToString());
                return "";
            }
        }
    }
}
