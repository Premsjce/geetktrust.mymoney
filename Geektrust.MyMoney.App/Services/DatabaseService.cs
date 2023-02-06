using Geektrust.MyMoney.App.Contracts;
using Geektrust.MyMoney.App.DAL;
using Geektrust.MyMoney.App.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Geektrust.MyMoney.App.Services
{
    internal class DatabaseService : IDBService
    {
        private PortfolioDataStore _portfolioDataStore;
        public DatabaseService()
        {
            _portfolioDataStore = new PortfolioDataStore();
        }

        public Task<bool> CreateOrUpdate(Portfolio portfolio)
        {
            if (_portfolioDataStore.PortfolioItems.Any(x => x.Month == portfolio.Month))
                _portfolioDataStore.PortfolioItems.Remove(portfolio);

            _portfolioDataStore.PortfolioItems.Add(portfolio);

            return Task.FromResult(true);
        }

        public Task<bool> Delete(string month)
        {
            var portfolio = _portfolioDataStore.PortfolioItems.FirstOrDefault(x => x.Month == month);

            if (portfolio != null)
                _portfolioDataStore.PortfolioItems.Remove(portfolio);

            return Task.FromResult(true);
        }

        public Task<Portfolio> Get(string month)
        {
            var portfolio = _portfolioDataStore.PortfolioItems.FirstOrDefault(x => x.Month == month);

            return Task.FromResult(portfolio);
        }

        public Task<int> GetCount()
        {
            return Task.FromResult(_portfolioDataStore.PortfolioItems.Count);
        }
    }
}
