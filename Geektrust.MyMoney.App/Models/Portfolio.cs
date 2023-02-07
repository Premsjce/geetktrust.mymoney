using System.Collections.Generic;

namespace Geektrust.MyMoney.App.Models
{
    internal class Portfolio
    {
        public string Month { get; set; }
        public int TotalValue { get; set; }
        public IList<AssetDetails> AdjustedDetails { get; set; }


        public Portfolio(string month, IList<AssetDetails> adjustedDetails, int totalvalue)
        {
            Month = month;
            AdjustedDetails = adjustedDetails;
            TotalValue= totalvalue;
        }
    }
}
