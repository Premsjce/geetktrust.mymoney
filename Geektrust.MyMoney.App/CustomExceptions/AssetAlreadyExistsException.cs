using System;

namespace Geektrust.MyMoney.App.CustomExceptions
{
    internal class AssetAlreadyExistsException : Exception
    {
        public AssetAlreadyExistsException() : base()
        {

        }

        public AssetAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
