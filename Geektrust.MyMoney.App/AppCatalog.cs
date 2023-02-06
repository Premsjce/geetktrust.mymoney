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
        public AppCatalog()
        {
            _allocationService = new AllocationService();
            _dBService = new DatabaseService();
            _changeService = new ChangeService(_dBService, _allocationService);
            _portfolioService = new PortfolioService(_dBService, _allocationService);

        }

        public async Task Driver(string[] inputCommands)
        {
            foreach(var command in inputCommands)
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
            var allocationList = ParseAssetList(details);
            await _allocationService.UpdateInitAllocations(allocationList);
        }

        private async Task ProcessSIPCommand(string[] details)
        {
            var sipList = ParseAssetList(details);
            await _allocationService.UpdateSipAllocations(sipList);

        }

        private async Task ProcessChangeCommand(string[] details)
        {
            var month = details[4];
            var assetDetails = ParseAssetList(details);
            await _changeService.Update(month, assetDetails);
        }

        private async Task ProcessBalanceCommad(string[] details)
        {
            var result = await _portfolioService.Balance(details[1]);
            var gold = result.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;
            var equity = result.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;
            var debt = result.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;

            Console.WriteLine($"{Convert.ToInt32(debt)} {Convert.ToInt32(equity)} {Convert.ToInt32(gold)}");
        }

        private async Task ProcessRebalanceCommand(string[] details)
        {
            var result = await _portfolioService.Rebalance();
            if(result == null)
            {
                Console.WriteLine("CANNOT_REBALANCE");
                return;
            }

            var gold = result.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;
            var equity = result.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;
            var debt = result.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;

            Console.WriteLine($"{Convert.ToInt32(debt)} {Convert.ToInt32(equity)} {Convert.ToInt32(gold)}");
        }


        private IList<Asset> ParseAssetList(string[] details)
        {
            var temp = TrimPercent(details[1]);
            var equityValue = float.Parse(temp);
            var debtValue = float.Parse(TrimPercent(details[2]));
            var goldValue = float.Parse(TrimPercent(details[3]));

            var assetList = new List<Asset>()
            {
                new Asset(AssetNames.EQUITY, equityValue),
                new Asset(AssetNames.DEBT, debtValue),
                new Asset(AssetNames.GOLD, goldValue)
            };

            return assetList;
        }

        private string TrimPercent(string val)
        {
            if (!val.Contains("%"))
                return val;

            return val.Remove(val.Length-1);
        }
    }
}
