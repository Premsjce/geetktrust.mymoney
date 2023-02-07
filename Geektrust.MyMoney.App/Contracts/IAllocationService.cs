using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Contracts
{
    internal interface IAllocationService
    {
        Task AddAllocationDetails(AssetType assetType, int amount);
        Task<int> GetAllocationAmount(AssetType assetType);
        Task<float> GetAllocationPercentage(AssetType assetType);
        Task<int> GetTotalAllocationAmount();
    }
}
