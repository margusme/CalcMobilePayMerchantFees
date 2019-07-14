using System;
using System.IO;
using System.Text;
using CalcMobilePayMerchantFees.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Moq;
using Moq.Protected;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionSource;

namespace CalcMobilePayMerchantFeesTest.TransactionSource
{
    [TestClass]
    public class TransactionsTextFileReaderTest
    {
        [TestMethod]
        public void TestConstructorShouldThrowException()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            Should.Throw<ArgumentNullException>(() => new TransactionsTextFileReader(null, transactionStringReaderMock.Object, "C:\\Temp\\transactions.txt"))
                .Message.ShouldContain("logger");

            Should.Throw<ArgumentNullException>(() => new TransactionsTextFileReader(loggerMock.Object, null, "C:\\Temp\\transactions.txt"))
                .Message.ShouldContain("transactionStringReader");
        }

        [TestMethod]
        public void TestConstructorShouldSetError()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new TransactionsTextFileReader(loggerMock.Object, transactionStringReaderMock.Object, "");
            transactionsFileReader.HasError().ShouldBe(true);

            transactionsFileReader = new TransactionsTextFileReader(loggerMock.Object, transactionStringReaderMock.Object, "Very1Long2File3Name4That5Should6Definitely7Not8Exist9On10Default11Path12.txt");
            transactionsFileReader.HasError().ShouldBe(true);
        }

        [TestMethod]
        public void TestReadNextTransactionShouldReturnCorrectValue()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new Mock<TransactionsTextFileReader>(loggerMock.Object, transactionStringReaderMock.Object, "")
            {
                CallBase = true
            };
            var transactionObject = new TransactionObject()
            {
                TransactionDate = DateTime.MinValue,
                MerchantName = "test",
                TransactionAmount = 10,
                TransactionPercentageFee = 0
            };
            transactionsFileReader.Object.ReadNextTransaction().ShouldBe(null);

            transactionsFileReader.Setup(method => method.HasMoreTransactions()).Returns(true);

            transactionsFileReader.Protected().Setup<TransactionObject>("GetTransactionFromLine", ItExpr.IsAny<string>()).Returns(transactionObject);
            transactionsFileReader.Object.ReadNextTransaction().ShouldBe(null);

            SetGetTransactionFileReader(transactionsFileReader);

            transactionsFileReader.Object.ReadNextTransaction().ShouldBe(transactionObject);
        }

        [TestMethod]
        public void TestHasMoreTransactionsShouldReturnCorrectValue()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new Mock<TransactionsTextFileReader>(loggerMock.Object, transactionStringReaderMock.Object, "")
            {
                CallBase = true
            };

            transactionsFileReader.Object.HasMoreTransactions().ShouldBe(false);

            SetGetTransactionFileReader(transactionsFileReader);

            transactionsFileReader.Object.HasMoreTransactions().ShouldBe(true);
        }

        [TestMethod]
        public void TestHasErrorShouldReturnCorrectValue()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new Mock<TransactionsTextFileReaderChild>(loggerMock.Object, transactionStringReaderMock.Object, "")
            {
                CallBase = true
            };

            //Should be true for the reason that passed empty file name into constructor
            transactionsFileReader.Object.HasError().ShouldBe(true);

            transactionsFileReader.Object.ChildFileOperationsHaveErrors = false;

            transactionsFileReader.Object.HasError().ShouldBe(false);
        }

        [TestMethod]
        public void TestOpenShouldReturnCorrectValue()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new Mock<TransactionsTextFileReader>(loggerMock.Object, transactionStringReaderMock.Object, "")
            {
                CallBase = true
            };

            //Should be true for the reason that passed empty file name into constructor
            transactionsFileReader.Object.Open().ShouldBe(false);

            SetGetTransactionFileReader(transactionsFileReader, true);

            transactionsFileReader.Object.Open().ShouldBe(true);
        }

        [TestMethod]
        public void TestCloseShouldSetErrorPropertyCorrect()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new Mock<TransactionsTextFileReaderChild>(loggerMock.Object, transactionStringReaderMock.Object, "")
            {
                CallBase = true
            };

            //Should be true for the reason that passed empty file name into constructor
            transactionsFileReader.Object.ChildFileOperationsHaveErrors.ShouldBe(true);

            transactionsFileReader.Object.Close();

            transactionsFileReader.Object.ChildFileOperationsHaveErrors.ShouldBe(false);
        }

        [TestMethod]
        public void TestGetTransactionsSourceNameShouldReturnCorrectValue()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileName = "Very1Long2File3Name4That5Should6Definitely7Not8Exist9On10Default11Path12.txt";
            var transactionsFileReader = new Mock<TransactionsTextFileReader>(loggerMock.Object, transactionStringReaderMock.Object, transactionsFileName)
            {
                CallBase = true
            };

            //Should be true for the reason that passed empty file name into constructor
            transactionsFileReader.Object.GetTransactionsSourceName().ShouldBe(transactionsFileName);
        }

        [TestMethod]
        public void TestGetTransactionFromLineShouldReturnCorrectValue()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionStringReaderMock = new Mock<ITransactionStringReader>();

            var transactionsFileReader = new Mock<TransactionsTextFileReaderChild>(loggerMock.Object, transactionStringReaderMock.Object, "")
            {
                CallBase = true
            };

            transactionsFileReader.Object.GetTransactionFromLine("").ShouldBe(null);

            var transactionObject = new TransactionObject()
            {
                TransactionDate = new DateTime(2018, 1, 1),
                MerchantName = "test",
                TransactionAmount = 10,
                TransactionPercentageFee = 0
            };
            var transactionLine = "2018-01-01 test 0.00";
            transactionStringReaderMock.Setup(method => method.ReadTransaction(transactionLine)).Returns(transactionObject);

            transactionsFileReader.Object.GetTransactionFromLine(transactionLine).ShouldBe(transactionObject);
        }

        private static void SetGetTransactionFileReader(Mock<TransactionsTextFileReader> transactionsFileReader, bool setOpenStreamReader = false)
        {
            string fakeFileContents = "Hello world";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);
            StreamReader streamReader = new StreamReader(fakeMemoryStream);
            transactionsFileReader.Protected().Setup<StreamReader>("GetTransactionFileReader")
                .Returns(streamReader);

            if (setOpenStreamReader)
            {
                transactionsFileReader.Protected().Setup<StreamReader>("OpenStreamReader")
                    .Returns(streamReader);
            }
        }
    }

    public class TransactionsTextFileReaderChild : TransactionsTextFileReader
    {
        public bool ChildFileOperationsHaveErrors
        {
            get { return FileOperationsHaveErrors; }
            set { FileOperationsHaveErrors = value; }
        }

        public TransactionsTextFileReaderChild(ICalcFeesLogger logger,
            ITransactionStringReader transactionStringReader, string transactionFileName) : base(logger,
            transactionStringReader, transactionFileName)
        {

        }

        public new TransactionObject GetTransactionFromLine(string line)
        {
            return base.GetTransactionFromLine(line);
        }
    }
}
