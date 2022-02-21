using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.OrderExceptions
{
    public class OrderAlreadyExistException: Exception
    {
        public OrderAlreadyExistException(string message)
       : base(message)
        {
        }
    }
}
