using System;
using System.Collections.Generic;
using System.Text;

namespace CalcMobilePayMerchantFees.Logging
{
    /// <summary>
    /// Interface for logger used throughout CalcMobilePayMerchantFees program
    /// </summary>
    public interface ICalcFeesLogger
    {
        /// <summary>
        /// Writes whole string line to the source (console, file etc.)
        /// </summary>
        /// <param name="line"></param>
        void WriteLine(string line);
    }
}
