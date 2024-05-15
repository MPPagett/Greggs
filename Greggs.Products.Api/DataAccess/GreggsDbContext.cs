using System.Collections.Generic;
using Greggs.Products.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Greggs.Products.Api.DataAccess;

public class GreggsDbContext : DbContext
{
    public IEnumerable<Product> Products = new List<Product>()
    {
        new() { Name = "Sausage Roll", Price = 1m },
        new() { Name = "Vegan Sausage Roll", Price = 1.1m },
        new() { Name = "Steak Bake", Price = 1.2m },
        new() { Name = "Yum Yum", Price = 0.7m },
        new() { Name = "Pink Jammie", Price = 0.5m },
        new() { Name = "Mexican Baguette", Price = 2.1m },
        new() { Name = "Bacon Sandwich", Price = 1.95m },
        new() { Name = "Coca Cola", Price = 1.2m }
    };

    public IEnumerable<CurrencyExchangeRate> CurrencyExchangeRates = new List<CurrencyExchangeRate>()
    {
        new() { CurrencyCode = "EUR", Conversion = 1.11m },
    };
}