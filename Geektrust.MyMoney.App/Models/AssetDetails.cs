using Geektrust.MyMoney.App.Constants;

namespace Geektrust.MyMoney.App.Models
{
    internal class AssetDetails
    {
        public AssetDetails(AssetType assetType, int value)
        {
            AssetType = assetType;
            Value = value;
        }

        public AssetType AssetType { get; }
        public int Value { get; }
    }
}
