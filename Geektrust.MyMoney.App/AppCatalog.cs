using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.Models;
using Geektrust.MyMoney.App.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App
{
    internal class AppCatalog
    {
        private IAllocationService _allocationService;
        private IPortfolioService _portfolioService;
        private IDBService _dBService;
        private IChangeService _changeService;
        private ISIPService _sipService;
        public AppCatalog()
        {
            _allocationService = new AllocationService();
            _dBService = new DatabaseService();
            _sipService = new SIPService();

            _changeService = new ChangeService(_dBService, _allocationService, _sipService);
            _portfolioService = new PortfolioService(_dBService, _allocationService);
        }

        public async Task Driver(string[] inputCommands)
        {
            foreach (var command in inputCommands)
            {
                if (string.IsNullOrEmpty(command))
                    return;

                var details = command.Split(' ');
                switch (details[0])
                {
                    case CommandNames.ALLOCATE:
                        await ProcessAllocationCommand(details);
                        break;
                    case CommandNames.SIP:
                        await ProcessSIPCommand(details);
                        break;
                    case CommandNames.CHANGE:
                        await ProcessChangeCommand(details);
                        break;
                    case CommandNames.BALANCE:
                        await ProcessBalanceCommad(details);
                        break;
                    case CommandNames.REBALANCE:
                        await ProcessRebalanceCommand(details);
                        return;
                }
            }
        }

        private async Task ProcessAllocationCommand(string[] details)
        {
            var assetDetails = ParseAssetDetails(details);
            foreach (var asset in assetDetails)
                await _allocationService.AddAllocationDetails(asset.AssetType, asset.Value);
        }

        private async Task ProcessSIPCommand(string[] details)
        {
            var sipList = ParseAssetDetails(details);
            foreach (var asset in sipList)
                await _sipService.AddSipDetails(asset.AssetType, asset.Value);
        }

        private async Task ProcessChangeCommand(string[] details)
        {
            var month = details[4];
            var assetDetails = ParseAssetPercentage(details);
            await _changeService.AdjustAfterChange(month, assetDetails);
        }

        private async Task ProcessBalanceCommad(string[] details)
        {
            var resultDetails = await _portfolioService.BalancedAssets(details[1]);
            var gold = resultDetails.FirstOrDefault(x => x.AssetType == AssetType.GOLD).Value;
            var equity = resultDetails.FirstOrDefault(x => x.AssetType == AssetType.EQUITY).Value;
            var debt = resultDetails.FirstOrDefault(x => x.AssetType == AssetType.DEBT).Value;

            Console.WriteLine($"{equity} {debt} {gold}");
        }

        private async Task ProcessRebalanceCommand(string[] details)
        {
            var result = await _portfolioService.RebalanceAssets();
            if (result == null)
            {
                Console.WriteLine("CANNOT_REBALANCE");
                return;
            }
            
            var equity = result.FirstOrDefault(x => x.AssetType == AssetType.EQUITY).Value;
            var debt = result.FirstOrDefault(x => x.AssetType == AssetType.DEBT).Value;
            var gold = result.FirstOrDefault(x => x.AssetType == AssetType.GOLD).Value;

            Console.WriteLine($"{equity} {debt} {gold}");
        }

        private IList<AssetDetails> ParseAssetDetails(string[] details)
        {
            var equity = new AssetDetails(AssetType.EQUITY, Convert.ToInt32(details[1]));
            var debt = new AssetDetails(AssetType.DEBT, Convert.ToInt32(details[2]));
            var gold = new AssetDetails(AssetType.GOLD, Convert.ToInt32(details[3]));

            return new List<AssetDetails>() { gold, debt, equity };
        }

        private IList<AssetPercentage> ParseAssetPercentage(string[] details)
        {
            var equity = new AssetPercentage(AssetType.EQUITY, float.Parse(TrimPercent(details[1])));
            var debt = new AssetPercentage(AssetType.DEBT, float.Parse(TrimPercent(details[2])));
            var gold = new AssetPercentage(AssetType.GOLD, float.Parse(TrimPercent(details[3])));
            return new List<AssetPercentage>() { gold, debt, equity };
        }

        private string TrimPercent(string val)
        {
            if (!val.Contains("%"))
                return val;

            return val.Remove(val.Length - 1);
        }
    }
}
