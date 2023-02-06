using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Contracts
{
    internal interface IPortfolioService
    {
        Task<IList<Asset>> Balance(string month);
        Task<IList<Asset>> Rebalance();
    }
}
