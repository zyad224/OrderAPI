using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.OrderExceptions
{
    public class OrderNotExistException: Exception
    {
        public OrderNotExistException(string message)
        : base(message)
        {
        }
    }
}
