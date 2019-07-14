using System;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.TransactionDestination;
using CalcMobilePayMerchantFees.TransactionSource;

namespace CalcMobilePayMerchantFees.TransactionProcessing
{
    /// <summary>
    /// Class is used to read payment transactions from any possible source, process transactions and write back data into any possible source
    /// </summary>
    public class TransactionsProcessor
    {
        private readonly ITransactionsReader _transactionsReader;
        private readonly ITransactionsWriter _transactionsWriter;
        private readonly ICalcFeesLogger _logger;

        protected const int MaxErrorCount = 10;

        /// <summary>
        /// Class constructor. 
        /// </summary>
        /// <param name="logger">Logger object</param>
        /// <param name="transactionsReader">Payment transactions reader from any possible source</param>
        /// <param name="transactionsWriter">Payment transactions writer to any possible source</param>
        public TransactionsProcessor(ICalcFeesLogger logger, ITransactionsReader transactionsReader, ITransactionsWriter transactionsWriter)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
            _transactionsReader = transactionsReader ?? throw new ArgumentNullException("transactionsReader");
            _transactionsWriter = transactionsWriter ?? throw new ArgumentNullException("transactionsWriter");
        }

        /// <summary>
        /// Opens source for reading transactions, processes these one by one and writes the result into output source
        /// </summary>
        /// <returns>true if transactions source and destination opening succeeded and could start to read transactions</returns>
        public bool ProcessAllTransactions()
        {
            //Transaction reader had error during construction
            if (_transactionsReader.HasError())
            {
                return false;
            }

            if (!TryOpenTransactionReader())
            {
                return false;
            }

            if (!TryOpenTransactionWriter())
            {
                return false;
            }

            int errorCount = 0;

            while (_transactionsReader.HasMoreTransactions())
            {
                if (!ProcessNextTransaction() && TransactionErrorCountExceeded(ref errorCount))
                {
                    _logger.WriteLine("Abort going through transactions");
                    break;
                }
            }

            _transactionsReader.Close();
            _transactionsWriter.Close();

            return true;
        }

        /// <summary>
        /// Reads next payment transaction object from the source, processes the result and writes into destination
        /// </summary>
        /// <returns>True if processing succeeded</returns>
        protected virtual bool ProcessNextTransaction()
        {
            var transactionObject = _transactionsReader.ReadNextTransaction();

            if (transactionObject != null)
            {
                transactionObject.TransactionPercentageFee = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
                return _transactionsWriter.WriteNextTransaction(transactionObject);
            }
            else
            {
                _transactionsWriter.WriteNextTransaction(null);
            }

            return false;
        }

        /// <summary>
        /// Detects if either reading or writing did not succeed and returns true if it did not succeed more than predefined MaxErrorCount times
        /// </summary>
        /// <param name="errorCount">Existing reading or writing error count</param>
        /// <returns>True if error count exceeded MaxErrorCount and reading or writing caused an error</returns>
        protected virtual bool TransactionErrorCountExceeded(ref int errorCount)
        {
            if (_transactionsReader.HasError() || _transactionsWriter.HasError())
            {
                _logger.WriteLine("Reading or writing next transaction resulted an error");
                errorCount++;

                if (errorCount > MaxErrorCount)
                {
                    _logger.WriteLine(string.Format("Transactions processing have already more than {0} errors",
                        MaxErrorCount));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to open payment transactions source for reading
        /// </summary>
        /// <returns>true if source opening succeeded</returns>
        protected virtual bool TryOpenTransactionReader()
        {
            if (!_transactionsReader.Open())
            {
                _logger.WriteLine("Could not open transactions source - " + _transactionsReader.GetTransactionsSourceName());
                //Close of what would have remained opened
                _transactionsReader.Close();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to open payment transactions destination for writing
        /// </summary>
        /// <returns>true if source opening succeeded</returns>
        protected virtual bool TryOpenTransactionWriter()
        {
            if (!_transactionsWriter.Open())
            {
                _logger.WriteLine("Could not open transactions destination for the results");
                //Close of what would have remained opened
                _transactionsWriter.Close();

                return false;
            }

            return true;
        }
    }
}
