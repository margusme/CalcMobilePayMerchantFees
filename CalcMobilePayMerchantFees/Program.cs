using System;
using System.IO;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionDestination;
using CalcMobilePayMerchantFees.TransactionProcessing;
using CalcMobilePayMerchantFees.TransactionSource;

namespace CalcMobilePayMerchantFees
{
    class Program
    {
        private const string TransactionsFileName = "transactions.txt";
        private static ICalcFeesLogger _logger;

        private static string _filePath;

        static void Main(string[] args)
        {
            _logger = new CalcFeesConsoleLogger();

            if (!ParseArguments(args))
            {
                return;
            }

            var fileReader = new TransactionsTextFileReader(_logger, new TransactionString3ColumnReader(_logger), _filePath);
            if (fileReader.HasError())
            {
                _logger.WriteLine("Could not initialise transactions file for reading - " + _filePath);
                return;
            }

            var processor = new TransactionsProcessor(_logger, fileReader, new TransactionsConsoleWriter(_logger, new TransactionString3ColumnWriter(_logger)));

            if (!processor.ProcessAllTransactions())
            {
                _logger.WriteLine("Could not process all transactions - " + _filePath);
            }
        }

        static bool ParseArguments(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                _logger.WriteLine("No path provided where to read transactions from, use current path");
                _filePath = Path.Combine(Directory.GetCurrentDirectory(), TransactionsFileName);

                return true;
            }

            string fileDirectory = args[0];

            if (!Directory.Exists(fileDirectory))
            {
                _logger.WriteLine("Directory does not exist - " + fileDirectory);
                return false;
            }

            _filePath = Path.Combine(fileDirectory, TransactionsFileName);

            return true;
        }
    }
}
