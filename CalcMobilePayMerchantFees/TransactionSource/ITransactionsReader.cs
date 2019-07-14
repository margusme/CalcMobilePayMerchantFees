using CalcMobilePayMerchantFees.Models;

namespace CalcMobilePayMerchantFees.TransactionSource
{
    /// <summary>
    /// Interface for dealing with different types of sources where transactions can be located
    /// </summary>
    public interface ITransactionsReader
    {
        bool Open();
        TransactionObject ReadNextTransaction();
        bool HasMoreTransactions();
        bool HasError();
        void Close();
        string GetTransactionsSourceName();
    }
}
