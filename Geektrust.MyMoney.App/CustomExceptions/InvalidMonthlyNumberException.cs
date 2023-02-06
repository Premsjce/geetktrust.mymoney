using System;

namespace Geektrust.MyMoney.App.CustomExceptions
{
    internal class InvalidMonthlyNumberException : Exception
    {
        public InvalidMonthlyNumberException() : base()
        {

        }

        public InvalidMonthlyNumberException(string message) : base(message)
        {

        }
    }
}
