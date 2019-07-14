using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using CalcMobilePayMerchantFees.Models;
using CalcMobilePayMerchantFees.TransactionProcessing;
using CalcMobilePayMerchantFees.TransactionProcessing.Clients;

namespace CalcMobilePayMerchantFeesTest.TransactionProcessing.Clients
{
    [TestClass]
    public class TransactionFeeCalculatorCircleKTest
    {
        [TestMethod]
        public void TestCalculateTransactionFeeShouldReturnCorrectValue()
        {
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 1, 1), MerchantName = "Circle_K", TransactionAmount = 200 };
            var calculator = new TransactionFeeCalculatorCircleK();

            var result = calculator.CalculateTransactionFee(null);
            result.ShouldBe(0.00m);

            result = calculator.CalculateTransactionFee(transactionObject);
            result.ShouldBe(1.60m);

            transactionObject.TransactionAmount = 1999;
            result = calculator.CalculateTransactionFee(transactionObject);
            result.ShouldBe(15.99m);
        }

        [TestMethod]
        public void TestCalculateAdditionalFirstDayFeeShouldReturnCorrectValue()
        {
            var calculator = new TransactionFeeCalculatorCircleK();

            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 4, 1), MerchantName = "Circle_K", TransactionAmount = 0 };

            var result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(0.00m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 4, 1), MerchantName = "Circle_K", TransactionAmount = 200 };

            result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(0.00m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 9, 1), MerchantName = "Circle_K", TransactionAmount = 200 };

            result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(29.00m);

            result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(0.00m);
        }
    }
}
