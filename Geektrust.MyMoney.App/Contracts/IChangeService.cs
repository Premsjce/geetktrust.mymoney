using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Contracts
{
    internal interface IChangeService
    {
        Task<bool> AdjustAfterChange(string month, IList<AssetPercentage> assetPercentages);
    }
}
