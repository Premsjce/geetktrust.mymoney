using Geektrust.MyMoney.App.Constants;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Contracts
{
    internal interface ISIPService
    {
        Task AddSipDetails(AssetType assetType, int amount);
        Task<int> GetSipAmount(AssetType assetType);
        Task<int> GetTotalSipAmount();
    }
}
