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
    public class TransactionString3ColumnReaderTest
    {
        [TestMethod]
        public void TestConstructorShouldThrowException()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();

            Should.Throw<ArgumentNullException>(() => new TransactionString3ColumnReader(null))
                .Message.ShouldContain("logger");
        }

        [TestMethod]
        public void TestReadTransactionReturnsNull()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var columnsReaderMock = new Mock<TransactionString3ColumnReader>(loggerMock.Object)
            {
                CallBase = true
            };

            columnsReaderMock.Object.ReadTransaction("").ShouldBe(null);
            columnsReaderMock.Object.ReadTransaction(" ").ShouldBe(null);

            var transactionData = "2018-01-01 test 100 test";
            columnsReaderMock.Object.ReadTransaction(transactionData).ShouldBe(null);

            transactionData = "2018-13-01 test 100";
            columnsReaderMock.Object.ReadTransaction(transactionData).ShouldBe(null);

            transactionData = "ABC test 100";
            columnsReaderMock.Object.ReadTransaction(transactionData).ShouldBe(null);

            transactionData = "2018-01-01 test ABC";
            columnsReaderMock.Object.ReadTransaction(transactionData).ShouldBe(null);
        }

        [TestMethod]
        public void TestReadTransactionReturnsExpectedTransactionObject()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var columnsReaderMock = new Mock<TransactionString3ColumnReader>(loggerMock.Object)
            {
                CallBase = true
            };

            var merchantName = "test";
            var transactionYear = 2018;
            var transactionMonth = 1;
            var transactionDay = 2;
            var transactionAmount = 100;
            var transactionDateString = GetTransactionDateString(transactionYear, transactionMonth, transactionDay);
            var transactionData = transactionDateString + " " + merchantName + " " + transactionAmount;
            columnsReaderMock.Object.ReadTransaction(transactionData).ShouldNotBe(null);
            columnsReaderMock.Object.ReadTransaction(transactionData).TransactionDate.Year.ShouldBe(transactionYear);
            columnsReaderMock.Object.ReadTransaction(transactionData).TransactionDate.Month.ShouldBe(transactionMonth);
            columnsReaderMock.Object.ReadTransaction(transactionData).TransactionDate.Day.ShouldBe(transactionDay);
            columnsReaderMock.Object.ReadTransaction(transactionData).MerchantName.ShouldBe(merchantName);
            columnsReaderMock.Object.ReadTransaction(transactionData).TransactionAmount.ShouldBe(transactionAmount);
        }

        [TestMethod]
        public void TestTryParseTransactionAmountShouldFail()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var columnsReaderMock = new Mock<TransactionString3ColumnReaderChild>(loggerMock.Object)
            {
                CallBase = true
            };

            long transactionAmount;
            columnsReaderMock.Object.TryParseTransactionAmount("", out transactionAmount).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionAmount(null, out transactionAmount).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionAmount("1234545454545454545454545454545454545454545454545", out transactionAmount).ShouldBe(false);
        }

        [TestMethod]
        public void TestTryParseTransactionAmountShouldPass()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var columnsReaderMock = new Mock<TransactionString3ColumnReaderChild>(loggerMock.Object)
            {
                CallBase = true
            };

            long transactionAmount;
            columnsReaderMock.Object.TryParseTransactionAmount("10", out transactionAmount).ShouldBe(true);
            transactionAmount.ShouldBe(10);
        }

        [TestMethod]
        public void TestTryParseTransactionDateShouldFail()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var columnsReaderMock = new Mock<TransactionString3ColumnReaderChild>(loggerMock.Object)
            {
                CallBase = true
            };

            DateTime transactionDate;
            columnsReaderMock.Object.TryParseTransactionDate("", out transactionDate).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionDate(null, out transactionDate).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionDate("ABC", out transactionDate).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionDate("123456", out transactionDate).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionDate("2018-13-01", out transactionDate).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionDate("2018-12-32", out transactionDate).ShouldBe(false);
            columnsReaderMock.Object.TryParseTransactionDate("2018-02-29", out transactionDate).ShouldBe(false);
        }

        [TestMethod]
        public void TestTryParseTransactionDateShouldPass()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var columnsReaderMock = new Mock<TransactionString3ColumnReaderChild>(loggerMock.Object)
            {
                CallBase = true
            };

            DateTime transactionDate;
            var transactionYear = 2018;
            var transactionMonth = 1;
            var transactionDay = 2;
            var transactionDateString = GetTransactionDateString(transactionYear, transactionMonth, transactionDay);
            columnsReaderMock.Object.TryParseTransactionDate(transactionDateString, out transactionDate).ShouldBe(true);
            transactionDate.Year.ShouldBe(transactionYear);
            transactionDate.Month.ShouldBe(transactionMonth);
            transactionDate.Day.ShouldBe(transactionDay);
        }

        private static string GetTransactionDateString(int transactionYear, int transactionMonth, int transactionDay)
        {
            return transactionYear + "-0" + transactionMonth + "-0" + transactionDay;
        }
    }

    public class TransactionString3ColumnReaderChild : TransactionString3ColumnReader
    {
        public TransactionString3ColumnReaderChild(ICalcFeesLogger logger) : base(logger)
        {
            
        }

        public new bool TryParseTransactionAmount(string transactionAmountString, out long amount)
        {
            return base.TryParseTransactionAmount(transactionAmountString, out amount);
        }

        public new bool TryParseTransactionDate(string transactionDateString, out DateTime transactionDate)
        {
            return base.TryParseTransactionDate(transactionDateString, out transactionDate);
        }
    }
}
