using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.CustomExceptions
{
    public class DatabaseSaveException: Exception
    {
        public DatabaseSaveException(string message)
        : base(message)
        {
        }
    }
}
