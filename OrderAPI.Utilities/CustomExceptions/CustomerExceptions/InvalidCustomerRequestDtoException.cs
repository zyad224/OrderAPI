using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.CustomerExceptions
{
    public class InvalidCustomerRequestDtoException:Exception
    {
        public InvalidCustomerRequestDtoException(string message)
        :base(message)
        {
        }
    }
}
