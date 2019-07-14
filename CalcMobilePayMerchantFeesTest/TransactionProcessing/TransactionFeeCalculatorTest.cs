using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionProcessing;

namespace CalcMobilePayMerchantFeesTest.TransactionProcessing
{
    [TestClass]
    public class TransactionFeeCalculatorTest
    {
        [TestMethod]
        public void TestCalculateTransactionFeeShouldReturnCorrectValue()
        {
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "SEVEN", TransactionAmount = 200 };

            var result = TransactionFeeCalculator.CalculateTransactionFee(null);
            result.ShouldBe((decimal)0.00);

            result = TransactionFeeCalculator.CalculateTransactionFee(transactionObject);
            result.ShouldBe((decimal)2.00);

            transactionObject.TransactionAmount = 1999;
            result = TransactionFeeCalculator.CalculateTransactionFee(transactionObject);
            result.ShouldBe((decimal)19.99);
        }
    }
}
