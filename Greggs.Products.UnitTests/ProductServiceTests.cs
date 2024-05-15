using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductServiceTests
{
    // not actually a mock due to the minimal setup, but would
    // be in actual implementation with dummy data generated
    private readonly GreggsDbContext _dbContextMock;
    private readonly Mock<ILogger<ProductService>> _loggerMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _dbContextMock = new GreggsDbContext();
        _loggerMock = new Mock<ILogger<ProductService>>();
        _productService = new ProductService(_dbContextMock, _loggerMock.Object);
    }

    [Fact]
    public async Task Get_WithDefaultPagingParameters_ReturnsProducts()
    {
        // Arrange
        // Act
        var result = await _productService.Get();

        // Assert
        Assert.Equal(5, result.Count());
        Assert.Equal("Sausage Roll", result.First().Name);
        Assert.Equal("Pink Jammie", result.Last().Name);
    }

    [Fact]
    public async Task Get_WithNegativePagingParameters_ReturnsProducts()
    {
        // Arrange
        // Act
        var result = await _productService.Get(-5, -5);

        // Assert
        Assert.Equal(5, result.Count());
        Assert.Equal("Sausage Roll", result.First().Name);
        Assert.Equal("Pink Jammie", result.Last().Name);
    }

    [Fact]
    public async Task Get_WithCurrencyConversion_AppliesConversionRate()
    {
        // Arrange
        // Act
        var result = await _productService.Get(currencyCode: "EUR");

        // Assert
        Assert.Equal(1.11M, result.First().Price);
        Assert.Equal(0.555M, result.Last().Price);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("123")]
    [InlineData("")]
    [InlineData(null)]
    public async Task Get_WithInvalidCurrencyCode_ReturnsDefaultProductsAndLogsError(string invalidCurrencyCode)
    {
        // Arrange
        // Act
        var result = await _productService.Get(currencyCode: invalidCurrencyCode);

        // Assert
        Assert.Equal(5, result.Count());
        Assert.Equal("Sausage Roll", result.First().Name);
        Assert.Equal("Pink Jammie", result.Last().Name);

        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == 
                $"Currency Code {invalidCurrencyCode} not configured for this site." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}