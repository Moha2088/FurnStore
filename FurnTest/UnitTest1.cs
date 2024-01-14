using Microsoft.EntityFrameworkCore;
using FurnStore.Data;
using FurnStore.Models;
using FurnStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FurnStore;

public class TestBase
{
    protected FurnStoreContext GetContext()
    {
        var options = new DbContextOptionsBuilder<FurnStoreContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new FurnStoreContext(options);
    }
}

[TestClass]
public class UnitTest1 : TestBase
{
    [TestMethod]
    public async Task Index_ReturnsViewWithProducts()
    {
        // Arrange
        using var context = GetContext();
        var controllerResult = new RentController(context);

        // Act
        var result = await controllerResult.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [TestMethod]
    public async Task GenPdf_Should_Return_RedirectActionResult_To_RentedProducts()
    {
        // Arrange
        using var context = GetContext();
        var controllerResult = new RentController(context);

        // Act
        var result = (RedirectToActionResult)await controllerResult.GenPdf();

        // Assert
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(nameof(controllerResult.RentedProducts));
    }

    // [TestMethod]
    // public async Task RentedProducts_Should_Return_View_WithRentedProducts()
    // {
    //     // Arrange
    //     var context = GetContext();
    //     var controllerResult = new RentController(context);
    //     // Act
    //     var result = (ViewResult) await controllerResult.RentedProducts();
    //
    //     // Assert
    //     result.Should().BeOfType<ViewResult>().Which.ContentType.Should().BeOfType<List<Product>>();
    //
    // }
}