using System.Collections.Generic;
using System.Threading.Tasks;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // I don't want to reconfigure the endpoint too much as it is not within the spec,
    // but if I were to create this from scratch I would add the paging data to the
    // request headers to keep the URL clean while still being able to pass the paging data
    [HttpGet]
    public async Task<IEnumerable<Product>> Get(int pageStart = 0, int pageSize = 5, string currencyCode = Constants.Currency.GreatBritishPound)
    {
        return await _productService.Get(pageStart, pageSize, currencyCode);
    }
}