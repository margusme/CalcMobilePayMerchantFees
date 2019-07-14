using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionProcessing;
using CalcMobilePayMerchantFees.TransactionProcessing.Clients;

namespace CalcMobilePayMerchantFeesTest.TransactionProcessing.Clients
{
    [TestClass]
    public class TransactionFeeCalculatorTeliaTest
    {
        [TestMethod]
        public void TestCalculateTransactionFeeShouldReturnCorrectValue()
        {
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "Telia", TransactionAmount = 200 };
            var calculator = new TransactionFeeCalculatorTelia();

            var result = TransactionFeeCalculator.CalculateTransactionFee(null);
            result.ShouldBe((decimal)0.00);

            result = calculator.CalculateTransactionObjectFee(transactionObject);
            result.ShouldBe((decimal)1.80);

            transactionObject.TransactionAmount = 1999;
            result = calculator.CalculateTransactionObjectFee(transactionObject);
            result.ShouldBe((decimal)17.99);
        }
    }
}
