using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.Helpers;
using Geektrust.MyMoney.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Services
{
    internal class ChangeService : IChangeService
    {
        private readonly IDBService _dBService;
        private readonly IAllocationService _allocationService;

        public ChangeService(IDBService dBService, IAllocationService allocationService)
        {
            _dBService = dBService;
            _allocationService = allocationService;
        }

        public async Task<bool> Update(string month, IList<Asset> assetDetails)
        {

            if (month == Months.JANUARY)
                return await UpdateJanMonth(assetDetails);

            return await UpdateOtherMonths(month, assetDetails);
        }

        private async Task<bool> UpdateJanMonth(IList<Asset> assetDetails)
        {
            var initialEquityWeights = await _allocationService.GetInitialAllocationFor(AssetNames.EQUITY);
            var initialGoldWeights = await _allocationService.GetInitialAllocationFor(AssetNames.GOLD);
            var initialDebtWeights = await _allocationService.GetInitialAllocationFor(AssetNames.DEBT);

            var initAssetWeight = new List<Asset>
            {
                new Asset(AssetNames.EQUITY, initialEquityWeights),
                new Asset(AssetNames.GOLD, initialGoldWeights),
                new Asset(AssetNames.DEBT, initialDebtWeights)
            };

            var goldChangeInPercent = assetDetails.First(x => x.Name == AssetNames.GOLD).Value;
            var debtChangeInPercent = assetDetails.First(x => x.Name == AssetNames.DEBT).Value;
            var equityChangeInPercent = assetDetails.First(x => x.Name == AssetNames.EQUITY).Value;

            var afterMarketChangeGold = initialGoldWeights + (initialGoldWeights / 100 * goldChangeInPercent);
            var afterMarketChangeDebt = initialDebtWeights + (initialDebtWeights / 100 * debtChangeInPercent);
            var afterMarketChangeEquity = initialEquityWeights + (initialEquityWeights / 100 * equityChangeInPercent);

            var totalInitialWeights = afterMarketChangeGold + afterMarketChangeDebt + afterMarketChangeEquity;

            var changedWeight = new List<Asset>
            {
                new Asset(AssetNames.EQUITY, initialEquityWeights + afterMarketChangeEquity),
                new Asset(AssetNames.GOLD, initialGoldWeights + afterMarketChangeGold),
                new Asset(AssetNames.DEBT, initialDebtWeights + afterMarketChangeDebt)
            };

            var newItem = new Portfolio
            {
                Month = Months.JANUARY,
                InitialWeight = initAssetWeight,
                ChangedPercent = null,
                ChangedWeight = changedWeight,
                TotalValue = Convert.ToInt32(totalInitialWeights)
            };

            return await _dBService.CreateOrUpdate(newItem);
           
        }


        private async Task<bool> UpdateOtherMonths(string month, IList<Asset> assetDetails)
        {
            var prevMonth = MonthHelper.GetPreviousMonth(month);
            var prevDetails = await _dBService.Get(prevMonth);
            if (prevDetails == null)
            {
                return false;
            }

            var sipEquityWeights = await _allocationService.GetSIPAllocationFor(AssetNames.EQUITY);
            var sipGoldWeights = await _allocationService.GetSIPAllocationFor(AssetNames.GOLD);
            var sipDebtWeights = await _allocationService.GetSIPAllocationFor(AssetNames.DEBT);

            var existingGoldWeight = prevDetails.ChangedWeight.FirstOrDefault(x => x.Name == AssetNames.GOLD).Value;
            var existingEquityWeight = prevDetails.ChangedWeight.FirstOrDefault(x => x.Name == AssetNames.EQUITY).Value;
            var existingDebtWeight = prevDetails.ChangedWeight.FirstOrDefault(x => x.Name == AssetNames.DEBT).Value;

            var afterSipGold = existingGoldWeight + sipGoldWeights;
            var afterSipEquity = existingEquityWeight + sipEquityWeights;
            var afterSipDebt = existingDebtWeight + sipDebtWeights;

            var goldChangeInPercent = assetDetails.First(x => x.Name == AssetNames.GOLD).Value;
            var debtChangeInPercent = assetDetails.First(x => x.Name == AssetNames.DEBT).Value;
            var equityChangeInPercent = assetDetails.First(x => x.Name == AssetNames.EQUITY).Value;

            var afterMarketChangeGold = afterSipGold + (afterSipGold / 100  * goldChangeInPercent);
            var afterMarketChangeDebt = afterSipDebt + (afterSipGold / 100  * debtChangeInPercent);
            var afterMarketChangeEquity = afterSipEquity + (afterSipGold / 100  * equityChangeInPercent);

            var totalWeight = afterMarketChangeDebt + afterMarketChangeEquity + afterMarketChangeGold;

            var changedWeights = new List<Asset>()
            {
                 new Asset(AssetNames.GOLD, afterMarketChangeGold),
                 new Asset(AssetNames.EQUITY, afterMarketChangeEquity),
                 new Asset(AssetNames.DEBT, afterMarketChangeDebt),
            };

            var updatedItem = new Portfolio
            {
                Month = month,
                InitialWeight = prevDetails.ChangedWeight,
                ChangedWeight = changedWeights,
                ChangedPercent = assetDetails,
                TotalValue = Convert.ToInt32(totalWeight)
            };
            return await _dBService.CreateOrUpdate(updatedItem);
        }

    }
}
