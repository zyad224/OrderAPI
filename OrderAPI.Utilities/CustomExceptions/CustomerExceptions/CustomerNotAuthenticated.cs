using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions.CustomerExceptions
{
    public class CustomerNotAuthenticated: Exception
    {
        public CustomerNotAuthenticated(string message)
        : base(message)
        {
        }
    }
}
