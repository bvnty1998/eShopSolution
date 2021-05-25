using System;

namespace eShopSolution.Utilities
{
   public class eShopException : Exception
    {
        public eShopException()
        {
        }

        public eShopException(string message)
            : base(message)
        {
        }

        public eShopException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
