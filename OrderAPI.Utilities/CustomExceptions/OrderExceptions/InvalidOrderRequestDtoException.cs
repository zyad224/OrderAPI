using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.OrderExceptions
{
    public class InvalidOrderRequestDtoException:Exception
    {
        public InvalidOrderRequestDtoException(string message)
        : base(message)
        {
        }
    }
}
