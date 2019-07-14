using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionProcessing;

namespace CalcMobilePayMerchantFeesTest.TransactionProcessing
{
    [TestClass]
    public class TransactionMerchantClassifierTest
    {
        [TestMethod]
        public void TestCalculateTransactionFeeByMerchantShouldReturnCorrectValue()
        {
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "Telia", TransactionAmount = 200 };

            var result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(null);
            result.ShouldBe(0.00m);

            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(30.80m);

            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(1.80m);

            transactionObject.TransactionAmount = 1999;
            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(17.99m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "Circle_K", TransactionAmount = 200 };

            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(30.60m);

            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(1.60m);

            transactionObject.TransactionAmount = 1999;
            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(15.99m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "Teli", TransactionAmount = 200 };

            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(31.00m);

            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(2.00m);

            transactionObject.TransactionAmount = 1999;
            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe(19.99m);
        }
    }
}
