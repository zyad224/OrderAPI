using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions
{
    public class CustomerAlreadyExistException: Exception
    {
        public CustomerAlreadyExistException(string message)
      : base(message)
        {
        }
    }
}
