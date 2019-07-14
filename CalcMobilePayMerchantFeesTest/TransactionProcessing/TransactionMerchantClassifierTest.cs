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

            var result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe((decimal)0.00);

            transactionObject.TransactionAmount = 1999;
            result = TransactionMerchantClassifier.CalculateTransactionFeeByMerchant(transactionObject);
            result.ShouldBe((decimal)0.00);
        }
    }
}
