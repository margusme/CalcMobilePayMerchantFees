using System;
using System.Collections.Generic;
using System.Text;

namespace CalcMobilePayMerchantFees.Logging
{
    /// <summary>
    /// Console logger logger used throughout CalcMobilePayMerchantFees program
    /// </summary>
    public class CalcFeesConsoleLogger : ICalcFeesLogger
    {
        /// <summary>
        /// Writes whole line to the console
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line)
        {
            //If exception is thrown here then don't catch it to make it visible
            Console.WriteLine(line);
        }
    }
}
