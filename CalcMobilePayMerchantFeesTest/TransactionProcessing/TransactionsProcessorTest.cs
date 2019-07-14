using System;
using CalcMobilePayMerchantFees.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Moq;
using Moq.Protected;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionDestination;
using CalcMobilePayMerchantFees.TransactionProcessing;
using CalcMobilePayMerchantFees.TransactionSource;

namespace CalcMobilePayMerchantFeesTest.TransactionProcessing
{
    [TestClass]
    public class TransactionsProcessorTest
    {
        [TestMethod]
        public void TestConstructorShouldThrowException()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionsReaderMock = new Mock<ITransactionsReader>();
            var transactionsWriterMock = new Mock<ITransactionsWriter>();

            Should.Throw<ArgumentNullException>(() => new TransactionsProcessor(null, transactionsReaderMock.Object, transactionsWriterMock.Object))
                .Message.ShouldContain("logger");

            Should.Throw<ArgumentNullException>(() => new TransactionsProcessor(loggerMock.Object, null, transactionsWriterMock.Object))
                .Message.ShouldContain("transactionsReader");

            Should.Throw<ArgumentNullException>(() => new TransactionsProcessor(loggerMock.Object, transactionsReaderMock.Object, null))
                .Message.ShouldContain("transactionsWriter");
        }

        [TestMethod]
        public void TestProcessAllTransactionsShouldReturnRightResult()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionsReaderMock = new Mock<ITransactionsReader>();
            var transactionsWriterMock = new Mock<ITransactionsWriter>();

            var transactionsProcessor = new Mock<TransactionsProcessor>(loggerMock.Object, transactionsReaderMock.Object, transactionsWriterMock.Object);
            var processTransactionsResult = transactionsProcessor.Object.ProcessAllTransactions();
            processTransactionsResult.ShouldBe(false);

            transactionsReaderMock.Setup(method => method.HasError()).Returns(true);
            processTransactionsResult.ShouldBe(false);

            transactionsProcessor.Protected().Setup<bool>("TryOpenTransactionReader").Returns(true);
            processTransactionsResult.ShouldBe(false);

            transactionsReaderMock.Setup(method => method.HasError()).Returns(false);
            transactionsProcessor.Protected().Setup<bool>("TryOpenTransactionWriter").Returns(true);
            //Should ever the default behavior change for not yet mocked methods, will return false anyway from HasMoreTransactions
            transactionsReaderMock.Setup(method => method.HasMoreTransactions()).Returns(false);

            processTransactionsResult = transactionsProcessor.Object.ProcessAllTransactions();
            processTransactionsResult.ShouldBe(true);
        }

        [TestMethod]
        public void TestProcessAllTransactionsShouldBreakTheCycle()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionsReaderMock = new Mock<ITransactionsReader>();
            var transactionsWriterMock = new Mock<ITransactionsWriter>();

            var transactionsProcessor = new Mock<TransactionsProcessor>(loggerMock.Object, transactionsReaderMock.Object, transactionsWriterMock.Object);

            transactionsProcessor.Protected().Setup<bool>("TryOpenTransactionReader").Returns(true);
            transactionsProcessor.Protected().Setup<bool>("TryOpenTransactionWriter").Returns(true);

            transactionsReaderMock.Setup(method => method.HasMoreTransactions()).Returns(true);
            transactionsProcessor.Protected().Setup<bool>("ProcessNextTransaction").Returns(false);
            transactionsProcessor.Protected().Setup<bool>("TransactionErrorCountExceeded", ItExpr.Ref<int>.IsAny).Returns(true);

            var processTransactionsResult = transactionsProcessor.Object.ProcessAllTransactions();
            processTransactionsResult.ShouldBe(true);
        }

        [TestMethod]
        public void TestProcessNextTransactionShouldReturnRightResult()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionsReaderMock = new Mock<ITransactionsReader>();
            var transactionsWriterMock = new Mock<ITransactionsWriter>();

            TransactionObject transactionObject = null;
            transactionsReaderMock.Setup(method => method.ReadNextTransaction()).Returns(transactionObject);

            var transactionsProcessor = new TransactionsProcessorChild(loggerMock.Object, transactionsReaderMock.Object, transactionsWriterMock.Object);
            var processTransactionsResult = transactionsProcessor.ProcessNextTransaction();
            processTransactionsResult.ShouldBe(false);
            transactionsWriterMock.Verify(method => method.WriteNextTransaction(transactionObject), Times.Once);

            transactionObject = new TransactionObject()
            {
                TransactionDate = DateTime.MinValue, MerchantName = "test", TransactionAmount = 10,
                TransactionPercentageFee = 0
            };
            transactionsReaderMock.Setup(method => method.ReadNextTransaction()).Returns(transactionObject);
            transactionsWriterMock.Setup(method => method.WriteNextTransaction(transactionObject)).Returns(true);
            processTransactionsResult = transactionsProcessor.ProcessNextTransaction();
            processTransactionsResult.ShouldBe(true);
            transactionsWriterMock.Verify(method => method.WriteNextTransaction(transactionObject), Times.Once);
        }

        [TestMethod]
        public void TestTransactionErrorCountExceededShouldReturnRightResult()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionsReaderMock = new Mock<ITransactionsReader>();
            var transactionsWriterMock = new Mock<ITransactionsWriter>();

            int errorCount = 10;

            var transactionsProcessor = new TransactionsProcessorChild(loggerMock.Object, transactionsReaderMock.Object, transactionsWriterMock.Object);
            var processTransactionsResult = transactionsProcessor.TransactionErrorCountExceeded(ref errorCount);
            processTransactionsResult.ShouldBe(false);

            transactionsReaderMock.Setup(method => method.HasError()).Returns(true);
            processTransactionsResult = transactionsProcessor.TransactionErrorCountExceeded(ref errorCount);
            processTransactionsResult.ShouldBe(true);

            transactionsReaderMock.Setup(method => method.HasError()).Returns(false);
            transactionsWriterMock.Setup(method => method.HasError()).Returns(true);
            processTransactionsResult = transactionsProcessor.TransactionErrorCountExceeded(ref errorCount);
            processTransactionsResult.ShouldBe(true);

            errorCount = 9;
            processTransactionsResult = transactionsProcessor.TransactionErrorCountExceeded(ref errorCount);
            processTransactionsResult.ShouldBe(false);
        }

        [TestMethod]
        public void TestTryOpenTransactionReaderShouldReturnRightResult()
        {
            var loggerMock = new Mock<ICalcFeesLogger>();
            var transactionsReaderMock = new Mock<ITransactionsReader>();
            var transactionsWriterMock = new Mock<ITransactionsWriter>();

            var transactionsProcessor = new TransactionsProcessorChild(loggerMock.Object, transactionsReaderMock.Object, transactionsWriterMock.Object);
            var processTransactionsResult = transactionsProcessor.TryOpenTransactionReader();
            processTransactionsResult.ShouldBe(false);

            transactionsReaderMock.Setup(method => method.Open()).Returns(true);
            processTransactionsResult = transactionsProcessor.TryOpenTransactionReader();
            processTransactionsResult.ShouldBe(true);
        }
    }

    class TransactionsProcessorChild : TransactionsProcessor
    {
        public TransactionsProcessorChild(ICalcFeesLogger logger, ITransactionsReader transactionsReader, ITransactionsWriter transactionsWriter) : base(logger, transactionsReader, transactionsWriter)
        {

        }

        public new bool ProcessNextTransaction()
        {
            return base.ProcessNextTransaction();
        }

        public new bool TransactionErrorCountExceeded(ref int errorCount)
        {
            return base.TransactionErrorCountExceeded(ref errorCount);
        }

        public new bool TryOpenTransactionReader()
        {
            return base.TryOpenTransactionReader();
        }
    }
}
