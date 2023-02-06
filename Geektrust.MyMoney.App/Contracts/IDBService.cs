using Geektrust.MyMoney.App.Models;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Contracts
{
    internal interface IDBService
    {
        Task<bool> CreateOrUpdate(Portfolio portfolio);
        Task<bool> Delete(string month);
        Task<Portfolio> Get(string month);
        Task<int> GetCount();
    }
}
