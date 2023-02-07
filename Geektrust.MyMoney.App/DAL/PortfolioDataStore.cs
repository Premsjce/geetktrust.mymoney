using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;

namespace Geektrust.MyMoney.App.DAL
{
    internal class PortfolioDataStore
    {
        public IList<Portfolio> PortfolioItems { get; }

        public PortfolioDataStore()
        {
            PortfolioItems = new List<Portfolio>();
        }
    }
}
