using Geektrust.MyMoney.App.Constants;

namespace Geektrust.MyMoney.App.Models
{
    internal class AssetPercentage
    {
        public AssetPercentage(AssetType assetType, float percent)
        {
            AssetType = assetType;
            Percent = percent;
        }

        public AssetType AssetType { get; }
        public float Percent { get; }
    }
}
