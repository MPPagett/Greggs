using Greggs.Products.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Greggs.Products.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> Get(int pageStart, int pageSize, string currencyCode);
    }
}