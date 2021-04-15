using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Emun
{
    public enum Status
    {
        IsActive,
        Active
    }

    public enum OrderStatus
    {
        InProgress,
        Confirmed,
        Shipping,
        Success,
        Canceled
    }

    public enum TransactionStatus
    {
        Success,
        Failed
    }
}
