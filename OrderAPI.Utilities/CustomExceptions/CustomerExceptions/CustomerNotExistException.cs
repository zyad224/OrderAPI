using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.CustomerExceptions
{
    public class CustomerNotExistException:Exception
    {
        public CustomerNotExistException(string message)
        : base(message)
        {
        }
    }
}
