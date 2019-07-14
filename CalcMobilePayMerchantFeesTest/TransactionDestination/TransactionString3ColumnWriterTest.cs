using System;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Moq;
using CalcMobilePayMerchantFees.Logging;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionDestination;

namespace CalcMobilePayMerchantFeesTest.TransactionDestination
{
    [TestClass]
    public class TransactionString3ColumnWriterTest
    {
        [TestMethod]
        public void TestConstructorShouldThrowException()
        {
            Should.Throw<ArgumentNullException>(() => new TransactionString3ColumnWriter(null))
                .Message.ShouldContain("logger");
        }

        [TestMethod]
        public void TestConstructorShouldPass()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            Should.NotThrow(() => new TransactionString3ColumnWriter(loggerMock.Object));
        }

        [TestMethod]
        public void TestWriteTransactionDataShouldReturnEmptyStringOnNull()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var writer = new TransactionString3ColumnWriter(loggerMock.Object);
            var line = writer.WriteTransactionData(null);
            line.ShouldBeEmpty();
        }

        [TestMethod]
        public void TestWriteTransactionDataShouldReturnEmptyStringOnException()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var writer = new TransactionString3ColumnWriter(loggerMock.Object);
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(550, 1, 1)};
            var currentCulture = Thread.CurrentThread.CurrentCulture;

            CultureInfo arSY = new CultureInfo("ar-SY");
            arSY.DateTimeFormat.Calendar = new HijriCalendar();
            Thread.CurrentThread.CurrentCulture = arSY;

            var line = writer.WriteTransactionData(transactionObject);
            line.ShouldBeEmpty();

            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [TestMethod]
        public void TestWriteTransactionDataShouldReturnRightString()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var writer = new TransactionString3ColumnWriter(loggerMock.Object);
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "Telia", TransactionPercentageFee = 2.00m};

            var line = writer.WriteTransactionData(transactionObject);
            line.ShouldMatch("2018-01-01 Telia 2.00");
        }
    }
}
