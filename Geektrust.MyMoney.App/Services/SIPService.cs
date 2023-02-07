using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.CustomExceptions;
using Geektrust.MyMoney.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Services
{
    internal class SIPService : ISIPService
    {
        private IList<AssetDetails> _assetDetails;
        private int _totalValue = 0;
        public SIPService()
        {
            _assetDetails = new List<AssetDetails>();
        }

        public Task<int> GetSipAmount(AssetType assetType)
        {
            var asset = _assetDetails.FirstOrDefault(x => x.AssetType == assetType);
            if (asset == null)
                throw new AssetDoesNotExistsException("No Sip has been added for this Asset type yet");

            return Task.FromResult(asset.Value);
        }

        public Task<int> GetTotalSipAmount()
        {
            return Task.FromResult(_totalValue);
        }

        public Task AddSipDetails(AssetType assetType, int amount)
        {
            var asset = _assetDetails.FirstOrDefault(x => x.AssetType == assetType);
            if (asset != null)
                throw new AssetAlreadyExistsException("SIP for this Asset already added");

            _assetDetails.Add(new AssetDetails(assetType, amount));
            _totalValue += amount;

            return Task.CompletedTask;
        }
    }
}
