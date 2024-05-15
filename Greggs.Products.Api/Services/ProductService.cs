using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greggs.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private GreggsDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(GreggsDbContext dbContext, ILogger<ProductService> logger) 
        { 
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> Get(int pageStart = Constants.Paging.DefaultPageStart, int pageSize = Constants.Paging.DefaultPageSize, string currencyCode = Constants.Currency.GreatBritishPound)
        {
            pageStart = pageStart > 0 ? pageStart : Constants.Paging.DefaultPageStart;
            pageSize = pageSize > 0 ? pageSize : Constants.Paging.DefaultPageSize;

            var query = _dbContext.Products.Skip(pageStart).Take(pageSize);
            return GetProductsWithConvertedCurrency(query, currencyCode);
        }

        private IEnumerable<Product> GetProductsWithConvertedCurrency(IEnumerable<Product> products, string currencyCode)
        {
            var productList = products.ToList();
            var currencyConversion = _dbContext.CurrencyExchangeRates.FirstOrDefault(x => x.CurrencyCode.Trim().ToLower() == currencyCode?.Trim().ToLower());

            if (currencyConversion != null && !string.IsNullOrEmpty(currencyCode) && currencyCode != Constants.Currency.GreatBritishPound)
            {
                foreach (var item in productList)
                {
                    item.Price = item.Price * currencyConversion.Conversion;
                }
            } 
            else
            {
                _logger.LogError($"Currency Code {currencyCode} not configured for this site.");
            }

            return productList.AsEnumerable();
        }
    }
}
