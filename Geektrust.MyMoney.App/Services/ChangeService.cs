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
        private readonly ISIPService _sipService;

        public ChangeService(IDBService dBService, IAllocationService allocationService, ISIPService sipService)
        {
            _dBService = dBService;
            _allocationService = allocationService;
            _sipService = sipService;
        }

        public async Task<bool> AdjustAfterChange(string month, IList<AssetPercentage> changed)
        {
            if (month == Months.JANUARY)
            {
                var existingDetails = new List<AssetDetails>();
                foreach (AssetType assetType in Enum.GetValues(typeof(AssetType)))
                {
                    var allocationAmount = await _allocationService.GetAllocationAmount(assetType);
                    existingDetails.Add(new AssetDetails(assetType, Convert.ToInt32(allocationAmount)));
                }

                return await UpdatePortfolio(month, changed, existingDetails);
            }

            var prevMonth = MonthHelper.GetPreviousMonth(month);
            var prevMontDetails = await _dBService.Get(prevMonth);
            return await UpdatePortfolio(month, changed, prevMontDetails.AdjustedDetails);
        }

        private async Task<bool> UpdatePortfolio(string month, IList<AssetPercentage> changed, IList<AssetDetails> existingDetails)
        {
            var adjustedDetails = new List<AssetDetails>();
            var totalValue = 0;
            foreach (AssetType assetType in Enum.GetValues(typeof(AssetType)))
            {
                var existingValue = existingDetails.FirstOrDefault(x => x.AssetType == assetType).Value;
                var sipValue = month == Months.JANUARY ? 0 : await _sipService.GetSipAmount(assetType);
                var changedPercent = changed?.FirstOrDefault(x => x.AssetType == assetType)?.Percent;
                var adjustedValue = existingValue + sipValue;

                if (changedPercent != null)
                    adjustedValue = adjustedValue + Convert.ToInt32(adjustedValue / 100 * changedPercent.Value);

                totalValue += adjustedValue;

                adjustedDetails.Add(new AssetDetails(assetType, adjustedValue));
            }

            var portfolio = new Portfolio(month, adjustedDetails, totalValue);

            return await _dBService.CreateOrUpdate(portfolio);
        }
    }
}
