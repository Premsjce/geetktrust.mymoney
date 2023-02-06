using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Services
{
    internal class AllocationService : IAllocationService
    {
        private IList<Asset> _initialAllocationPercent;
        private IList<Asset> _sipAllocationValue;

        public Task UpdateInitAllocations(IList<Asset> initialAllocation)
        {
            _initialAllocationPercent = initialAllocation;
            return Task.CompletedTask;
        }

        public Task UpdateSipAllocations( IList<Asset> sipAllocationValue)
        {
            _sipAllocationValue = sipAllocationValue;
            return Task.CompletedTask;
        }

        public Task<float> GetInitialAllocationFor(string assetName)
        {
            var allocation = _initialAllocationPercent.FirstOrDefault(x => x.Name == assetName);
            return Task.FromResult(allocation.Value);
        }

        public Task<float> GetSIPAllocationFor(string assetName)
        {
            var allocation = _sipAllocationValue.FirstOrDefault(x => x.Name == assetName);
            return Task.FromResult(allocation.Value);
        }
    }
}
