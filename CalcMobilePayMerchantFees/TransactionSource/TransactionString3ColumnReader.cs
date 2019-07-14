using System;
using System.Globalization;
using System.Linq;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionSource
{
    /// <summary>
    /// Used to read data from 3-column string and output into payment transaction object
    /// </summary>
    public class TransactionString3ColumnReader : ITransactionStringReader
    {
        private readonly ICalcFeesLogger _logger;

        protected virtual char Separator => ' ';
        protected virtual string IncomingDateFormat => "yyyy-MM-dd";
        protected virtual CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="logger">Logger object</param>
        public TransactionString3ColumnReader(ICalcFeesLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
        }

        /// <summary>
        /// Gets payment transaction object from string data
        /// </summary>
        /// <param name="transactionData">String with 3-column transaction data</param>
        /// <returns>Payment transaction object</returns>
        public virtual TransactionObject ReadTransaction(string transactionData)
        {
            transactionData = transactionData.Trim();

            if (string.IsNullOrWhiteSpace(transactionData))
            {
                return null;
            }

            var lineElements = transactionData.Split(Separator).Where(elem => elem.Trim().Length > 0).ToArray();
            if (lineElements.Length != 3)
            {
                return null;
            }

            if (!TryParseTransactionDate(lineElements[0], out var transactionDate))
            {
                return null;
            }

            if (!TryParseTransactionAmount(lineElements[2], out var amount))
            {
                return null;
            }

            return new TransactionObject() { TransactionDate = transactionDate, MerchantName = lineElements[1], TransactionAmount = amount};
        }

        /// <summary>
        /// Tries to parse transaction amount from string
        /// </summary>
        /// <param name="transactionAmountString">String with amount data</param>
        /// <param name="amount">Output amount</param>
        /// <returns>True if extracting of amount succeeded</returns>
        protected virtual bool TryParseTransactionAmount(string transactionAmountString, out long amount)
        {
            amount = 0;

            try
            {
                amount = long.Parse(transactionAmountString);
            }
            catch (ArgumentNullException e)
            {
                _logger.WriteLine("TransactionAmount is null, exception: " + e.ToString());
                return false;
            }
            catch (FormatException e)
            {
                _logger.WriteLine("TransactionAmount " + transactionAmountString + " is not in correct format, exception: " + e.ToString());
                return false;
            }
            catch (OverflowException e)
            {
                _logger.WriteLine("TransactionAmount " + transactionAmountString + " is too big, exception: " + e.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to parse transaction date from string
        /// </summary>
        /// <param name="transactionDateString">String with date</param>
        /// <param name="transactionDate">Output date</param>
        /// <returns>True if extracting of payment transaction date succeeded</returns>
        protected virtual bool TryParseTransactionDate(string transactionDateString, out DateTime transactionDate)
        {
            transactionDate = DateTime.MinValue;

            try
            {
                transactionDate = DateTime.ParseExact(transactionDateString, IncomingDateFormat, CultureInfo);
            }
            catch (ArgumentNullException e)
            {
                _logger.WriteLine("TransactionDate is null, exception: " + e.ToString());
                return false;
            }
            catch (FormatException e)
            {
                _logger.WriteLine("TransactionDate " + transactionDateString + " is not in correct format, exception: " + e.ToString());
                return false;
            }

            return true;
        }
    }
}
