using System.Collections.Generic;

namespace Geektrust.MyMoney.App.Models
{
    internal class Portfolio
    {
        public string Month { get; set; }
        public IList<Asset> InitialWeight { get; set; }
        public IList<Asset> ChangedPercent { get; set; }
        public IList<Asset> ChangedWeight { get; set; }
        public int TotalValue { get; set; }
    }
}
