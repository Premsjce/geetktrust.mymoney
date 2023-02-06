using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.Helpers;
using Geektrust.MyMoney.App.Models;
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

        public async Task<IList<Asset>> Balance(string month)
        {
            var portfolio = await _dBService.Get(month);
            return portfolio.ChangedWeight;
        }

        public async Task<IList<Asset>> Rebalance()
        {
            var totalMonths = await _dBService.GetCount();
            if (totalMonths < 6)
                return null;

            var month = MonthHelper.GetMonthNameFromNumber(totalMonths % 12);
            var rebalancableMonth = MonthHelper.GetRebalancebleMonth(month);
            var portfolioItem = await _dBService.Get(rebalancableMonth);

            var goldInitAllocation = await _allocationService.GetInitialAllocationFor(AssetNames.GOLD);
            var debtInitAllocation = await _allocationService.GetInitialAllocationFor(AssetNames.DEBT);
            var equityInitAllocation = await _allocationService.GetInitialAllocationFor(AssetNames.EQUITY);

            var totalWeight = goldInitAllocation + debtInitAllocation + equityInitAllocation;
            var goldInitPercent = (goldInitAllocation / totalWeight) * 100;
            var debtInitPercent = (debtInitAllocation / totalWeight) * 100;
            var equityInitPercent = (equityInitAllocation / totalWeight) * 100;

            var goldAsset = new Asset(AssetNames.GOLD, GetValue(portfolioItem.TotalValue, goldInitPercent));
            var debtAsset = new Asset(AssetNames.DEBT, GetValue(portfolioItem.TotalValue, debtInitPercent));
            var equityAsset = new Asset(AssetNames.EQUITY, GetValue(portfolioItem.TotalValue, equityInitPercent));

            var rebalancedWeight = new List<Asset>()
            {
                goldAsset, debtAsset, equityAsset
            };

            return rebalancedWeight;
        }

        private float GetValue(int total, float percent) => (total / percent) * 100;
    }
}
