using Geektrust.MyMoney.App.Constants;

namespace Geektrust.MyMoney.App.Models
{
    internal class Asset
    {
        public Asset(string name, float value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public float Value { get; set; }

    }
}
