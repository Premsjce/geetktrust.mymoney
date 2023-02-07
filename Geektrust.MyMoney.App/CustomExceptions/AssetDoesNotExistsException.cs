using System;

namespace Geektrust.MyMoney.App.CustomExceptions
{
    internal class AssetDoesNotExistsException : Exception
    {
        public AssetDoesNotExistsException() : base()
        {

        }

        public AssetDoesNotExistsException(string message) : base(message)
        {

        }
    }
}
