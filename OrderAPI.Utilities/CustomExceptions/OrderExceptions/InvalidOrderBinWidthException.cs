using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.OrderExceptions
{
    public class InvalidOrderBinWidthException: Exception
    {
        public InvalidOrderBinWidthException(string message)
        : base(message)
        {
        }
    }
}
