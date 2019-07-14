using System;
using System.IO;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionSource
{
    /// <summary>
    /// Class is used for reading payment transactions from text file using given string format that transactionStringReader can understand
    /// </summary>
    public class TransactionsTextFileReader : ITransactionsReader
    {
        private readonly string _transactionFileName;
        private readonly ICalcFeesLogger _logger;
        private readonly ITransactionStringReader _transactionStringReader;

        protected StreamReader TransactionFileReader;
        protected bool FileOperationsHaveErrors { get; set; }

        /// <summary>
        /// Class constructor. Sets FileOperationsHaveErrors to true if payment transactions file name is empty or does not exist
        /// </summary>
        /// <param name="logger">Logger object</param>
        /// <param name="transactionStringReader">Payment transactions reader from specific format</param>
        /// <param name="transactionFileName">Payment transactions file name</param>
        public TransactionsTextFileReader(ICalcFeesLogger logger, ITransactionStringReader transactionStringReader, string transactionFileName)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
            _transactionStringReader = transactionStringReader ?? throw new ArgumentNullException("transactionStringReader");

            _transactionFileName = transactionFileName;
            FileOperationsHaveErrors = false;

            /* 
            * Sets FileOperationsHaveErrors to true for empty name or file not found already here
            */
            if (string.IsNullOrWhiteSpace(transactionFileName))
            {
                _logger.WriteLine("Cannot read from file if file name is empty");
                FileOperationsHaveErrors = true;
            }

            if (!File.Exists(transactionFileName))
            {
                _logger.WriteLine("File does not exist - " + transactionFileName);
                FileOperationsHaveErrors = true;
            }
        }

        /// <summary>
        /// Reads next payment transaction object from text line if there are possibly more transactions left
        /// </summary>
        /// <returns></returns>
        public virtual TransactionObject ReadNextTransaction()
        {
            if (HasMoreTransactions())
            {
                try
                {
                    return GetTransactionFromLine(GetTransactionFileReader().ReadLine());
                }
                catch (Exception e)
                {
                    FileOperationsHaveErrors = true;
                    _logger.WriteLine(e.ToString());
                }
            }

            return null;
        }

        /// <summary>
        /// Returns true if text stream reader has been initialized and has more data to read
        /// </summary>
        /// <returns>True if possibly more payment transactions are left</returns>
        public virtual bool HasMoreTransactions()
        {
            return GetTransactionFileReader() != null && !GetTransactionFileReader().EndOfStream;
        }

        /// <summary>
        /// Will return true if after creating TransactionsTextFileReader object cannot read file or
        /// cannot open file or after file opening cannot read from file.
        /// </summary>
        /// <returns>boolean indicating if file reading/opening/existing gives an error</returns>
        public virtual bool HasError()
        {
            return FileOperationsHaveErrors;
        }

        /// <summary>
        /// Open file stream for reading
        /// </summary>
        /// <returns>True if opening succeeds</returns>
        public virtual bool Open()
        {
            Close();

            try
            {
                SetTransactionFileReader(OpenStreamReader());
            }
            catch (Exception e)
            {
                FileOperationsHaveErrors = true;
                _logger.WriteLine("Could not open " + _transactionFileName + "exception: " + e.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Closes file stream and dsiposes used resources
        /// </summary>
        public virtual void Close()
        {
            GetTransactionFileReader()?.Dispose();

            FileOperationsHaveErrors = false;
        }

        /// <summary>
        /// Gets back file name with payment transactions data
        /// </summary>
        /// <returns></returns>
        public string GetTransactionsSourceName()
        {
            return _transactionFileName;
        }

        /// <summary>
        /// Reads payment transaction object from next text line using predefined _transactionStringReader
        /// </summary>
        /// <param name="line">Test line with payment transaction data</param>
        /// <returns>Payment transaction object</returns>
        protected virtual TransactionObject GetTransactionFromLine(string line)
        {
            return _transactionStringReader.ReadTransaction(line);
        }

        /// <summary>
        /// Opens a stream reader
        /// </summary>
        /// <returns>Opened stream reader</returns>
        protected virtual StreamReader OpenStreamReader()
        {
            return File.OpenText(_transactionFileName);
        }

        /// <summary>
        /// Sets reader stream object to be used throughout the payment transactions processing
        /// </summary>
        /// <param name="transactionFileReader">Opened stream reader</param>
        protected virtual void SetTransactionFileReader(StreamReader transactionFileReader)
        {
            TransactionFileReader = transactionFileReader;
        }

        /// <summary>
        /// Returns already opened stream reader
        /// </summary>
        /// <returns>Already opened stream reader</returns>
        protected virtual StreamReader GetTransactionFileReader()
        {
            return TransactionFileReader;
        }
    }
}
