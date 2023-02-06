using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Contracts
{
    internal interface IAllocationService
    {
        Task UpdateInitAllocations(IList<Asset> initialAllocation);
        Task UpdateSipAllocations(IList<Asset> initialAllocation);
        Task<float> GetInitialAllocationFor(string assetName);
        Task<float> GetSIPAllocationFor(string assetName);
    }
}
