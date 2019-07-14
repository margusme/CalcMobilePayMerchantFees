using System;
using System.Collections.Generic;
using System.Text;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionSource;

namespace CalcMobilePayMerchantFees.TransactionDestination
{
    /// <summary>
    /// Used to write all payment transactions from object into console using specific format given by child class of ITransactionStringWriter
    /// </summary>
    public class TransactionsConsoleWriter : ITransactionsWriter
    {
        private readonly ICalcFeesLogger _logger;
        private readonly ITransactionStringWriter _transactionStringWriter;

        protected bool WriteOperationsHaveErrors { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="transactionStringWriter">payment transaction writer into string</param>
        public TransactionsConsoleWriter(ICalcFeesLogger logger, ITransactionStringWriter transactionStringWriter)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
            _transactionStringWriter = transactionStringWriter ?? throw new ArgumentNullException("transactionStringWriter");
        }

        /// <summary>
        /// Do nothing
        /// </summary>
        public void Close()
        {
            //Do nothing for the console
        }

        /// <summary>
        /// Returns true if console writing resulted an error before
        /// </summary>
        /// <returns>true if console writing resulted an error before</returns>
        public bool HasError()
        {
            return WriteOperationsHaveErrors;
        }

        /// <summary>
        /// Not in use, only resets console writing error to false
        /// </summary>
        /// <returns>Always true</returns>
        public bool Open()
        {
            WriteOperationsHaveErrors = false;

            return true;
        }

        /// <summary>
        /// Transforms payment transaction object into string representation using predefined transactionStringWriter
        /// </summary>
        /// <param name="transactionObject"></param>
        /// <returns>true if writing to the console succeeded</returns>
        public bool WriteNextTransaction(TransactionObject transactionObject)
        {
            string line = _transactionStringWriter.WriteTransactionData(transactionObject);

            try
            {
                Console.WriteLine(line);
            }
            catch (Exception e)
            {
                _logger.WriteLine("Writing to console encountered exception: " + e.ToString());
                WriteOperationsHaveErrors = true;

                return false;
            }

            return true;
        }
    }
}
