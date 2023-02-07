using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.Helpers;
using Geektrust.MyMoney.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Services
{
    internal class PortfolioService : IPortfolioService
    {
        private readonly IDBService _dBService;
        private readonly IAllocationService _allocationService;

        public PortfolioService(IDBService dBService, IAllocationService allocationService)
        {
            _dBService = dBService;
            _allocationService = allocationService;
        }

        public async Task<IList<AssetDetails>> BalancedAssets(string month)
        {
            var portfolio = await _dBService.Get(month);
            return portfolio.AdjustedDetails;
        }

        public async Task<IList<AssetDetails>> RebalanceAssets()
        {
            var totalMonths = await _dBService.GetCount();
            if (totalMonths < 6)
                return null;

            var month = MonthHelper.GetMonthNameFromNumber(totalMonths % 12);
            var rebalancableMonth = MonthHelper.GetRebalancebleMonth(month);
            var portfolioItem = await _dBService.Get(rebalancableMonth);

            var goldAllocationPercent = await _allocationService.GetAllocationPercentage(AssetType.GOLD);
            var debtAllocationPercent = await _allocationService.GetAllocationPercentage(AssetType.DEBT);
            var equityAllocationPercetn = await _allocationService.GetAllocationPercentage(AssetType.EQUITY);

            var goldAsset = new AssetDetails(AssetType.GOLD, Convert.ToInt32(portfolioItem.TotalValue / 100 * goldAllocationPercent));
            var debtAsset = new AssetDetails(AssetType.DEBT, Convert.ToInt32(portfolioItem.TotalValue / 100 * debtAllocationPercent));
            var equityAsset = new AssetDetails(AssetType.EQUITY, Convert.ToInt32(portfolioItem.TotalValue / 100 * equityAllocationPercetn));

            return new List<AssetDetails> { goldAsset, debtAsset, equityAsset };
        }
    }
}
