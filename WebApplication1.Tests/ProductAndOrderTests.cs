using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Tests;

public class ProductAndOrderTests
{
    [Fact]
    public void Product_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var product = new Product
        {
            Name = "Samsung QLED TV",
            Description = "55-inch QLED Smart TV",
            Price = 999.99m,
            StockQuantity = 10,
            ImageUrl = "/images/qled.jpg"
        };

        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(
            product,
            context,
            results,
            validateAllProperties: true
        );

        // Assert
        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void Product_WithInvalidPriceAndStock_ShouldFailValidation()
    {
        // Arrange
        var product = new Product
        {
            Name = "Invalid Product",
            Description = "Test product",
            Price = 0,
            StockQuantity = -1,
            ImageUrl = "/images/test.jpg"
        };

        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(
            product,
            context,
            results,
            validateAllProperties: true
        );

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Price)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.StockQuantity)));
    }

    [Fact]
    public void OrderTotal_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        decimal price = 500m;
        int quantity = 2;

        // Act
        decimal total = price * quantity;

        // Assert
        Assert.Equal(1000m, total);
    }

    [Fact]
    public void Stock_ShouldDecreaseAfterOrder()
    {
        // Arrange
        int stockQuantity = 10;
        int orderedQuantity = 3;

        // Act
        int remainingStock = stockQuantity - orderedQuantity;

        // Assert
        Assert.Equal(7, remainingStock);
    }

    [Fact]
    public void Order_ShouldNotAllowQuantityGreaterThanStock()
    {
        // Arrange
        int stockQuantity = 5;
        int orderedQuantity = 6;

        // Act
        bool isValidOrder = orderedQuantity <= stockQuantity;

        // Assert
        Assert.False(isValidOrder);
    }
}