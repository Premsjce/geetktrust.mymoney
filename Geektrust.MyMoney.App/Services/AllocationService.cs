using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.CustomExceptions;
using Geektrust.MyMoney.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Services
{
    internal class AllocationService : IAllocationService
    {      
        private IList<AssetDetails> _assetDetails;
        private int _totalValue = 0;

        public AllocationService()
        {
            _assetDetails = new List<AssetDetails>();
        }

        public Task AddAllocationDetails(AssetType assetType, int amount)
        {
            var asset = _assetDetails.FirstOrDefault(x => x.AssetType == assetType);
            if (asset != null)
                throw new AssetAlreadyExistsException("Alocation has been done for this Asset already");

            _assetDetails.Add(new AssetDetails(assetType, amount));
            _totalValue += amount;

            return Task.CompletedTask; throw new System.NotImplementedException();
        }

        public Task<int> GetAllocationAmount(AssetType assetType)
        {
            var asset = _assetDetails.FirstOrDefault(x => x.AssetType == assetType);
            if (asset == null)
                throw new AssetDoesNotExistsException("No Allocation has been made for this Asset type yet");

            return Task.FromResult(asset.Value);
        }

        public Task<int> GetTotalAllocationAmount()
        {
            return Task.FromResult(_totalValue);
        }

        public async Task<float> GetAllocationPercentage(AssetType assetType)
        {
            var amount = await GetAllocationAmount(assetType);
            return (float)(amount * 100)/_totalValue;
        }
    }
}
