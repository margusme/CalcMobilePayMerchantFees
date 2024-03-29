﻿using System;
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

            var calculator = new TransactionFeeCalculatorChild();

            var result = calculator.CalculateTransactionFee(null);
            result.ShouldBe(0.00m);

            result = calculator.CalculateTransactionFee(transactionObject);
            result.ShouldBe(2.00m);

            transactionObject.TransactionAmount = 1999;
            result = calculator.CalculateTransactionFee(transactionObject);
            result.ShouldBe(19.99m);
        }

        [TestMethod]
        public void TestCalculateAdditionalFirstDayFeeShouldReturnCorrectValue()
        {
            var calculator = new TransactionFeeCalculatorChild();

            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 2, 1), MerchantName = "SEVEN", TransactionAmount = 0 };

            var result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(0.00m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 2, 1), MerchantName = "SEVEN", TransactionAmount = 200 };

            result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(0.00m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 7, 1), MerchantName = "SEVEN", TransactionAmount = 200 };

            result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(29.00m);

            result = calculator.CalculateAdditionalFirstDayFee(transactionObject);
            result.ShouldBe(0.00m);
        }

        [TestMethod]
        public void TestCalculateTotalTransactionFeeShouldReturnCorrectValue()
        {
            var calculator = TransactionFeeCalculator.Instance();

            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 2, 1), MerchantName = "SEVEN-S", TransactionAmount = 0 };

            var result = calculator.CalculateTotalTransactionFee(transactionObject);
            result.ShouldBe(0.00m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 3, 1), MerchantName = "SEVEN-S", TransactionAmount = 200 };

            result = calculator.CalculateTotalTransactionFee(transactionObject);
            result.ShouldBe(31.00m);

            transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 3, 1), MerchantName = "SEVEN-S", TransactionAmount = 200 };

            result = calculator.CalculateTotalTransactionFee(transactionObject);
            result.ShouldBe(2.00m);
        }

        [TestMethod]
        public void TestMerchantHasTransactionsForMonthShouldReturnCorrectValue()
        {
            var transactionObject = new TransactionObject() { TransactionDate = new DateTime(2018, 10, 1), MerchantName = "SEVEN", TransactionAmount = 0 };

            var result = TransactionFeeCalculatorChild.MerchantHasTransactionsForMonth(null);
            result.ShouldBe(true);

            result = TransactionFeeCalculatorChild.MerchantHasTransactionsForMonth(transactionObject);
            result.ShouldBe(false);

            result = TransactionFeeCalculatorChild.MerchantHasTransactionsForMonth(transactionObject);
            result.ShouldBe(true);
        }
    }

    public class TransactionFeeCalculatorChild : TransactionFeeCalculator
    {
        public new static bool MerchantHasTransactionsForMonth(TransactionObject transactionObject)
        {
            return TransactionFeeCalculator.MerchantHasTransactionsForMonth(transactionObject);
        }

        public new decimal CalculateTransactionFee(TransactionObject transactionObject)
        {
            return base.CalculateTransactionFee(transactionObject);
        }

        public new decimal CalculateAdditionalFirstDayFee(TransactionObject transactionObject)
        {
            return base.CalculateAdditionalFirstDayFee(transactionObject);
        }
    }
}
