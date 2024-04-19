using FluentAssertions;
using FurnStore.Data;
using FurnStore.Controllers;
using FurnStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace FurnTest;

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
public class ProductControllerTest : TestBase
{
    [TestMethod]
    public async Task Create_Should_Return_Create_Page()
    {
        // Arrange
        var context = GetContext();

        var loggerResult = new Mock<ILogger<ProductsController>>();
        var controllerResult = new ProductsController(context, loggerResult.Object);

        // Act
        var result = controllerResult.Create();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [TestMethod]
    public async Task Create_Should_Return_Index_Page_With_New_Product()
    {
        // Arrange
        var context = GetContext();
        var loggerResult = new Mock<ILogger<ProductsController>>().Object;
        var controllerResult = new ProductsController(context, loggerResult);
        var product = new Product()
        {
            Name = "White Office Chair",
            Material = "Wood",
            Description = "White Chair perfect for office environments or for the dining table ",
            Price = 900,
            ImageUrl =
                "https://images.unsplash.com/photo-1517705008128-361805f42e86?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTk5fHxtaW5pbWFsJTIwZnVybml0dXJlfGVufDB8fDB8fHww",
            ShippingPrice = 30
        };

        // Act
        var result = (RedirectToActionResult)await controllerResult.Create(product);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be(nameof(controllerResult.Index));
    }

    [TestMethod]
    public async Task DeleteConfirmed_Should_Redirect_To_Index()
    {
        // Arrange
        var context = GetContext();
        var loggerResult = new Mock<ILogger<ProductsController>>().Object;
        var controllerResult = new ProductsController(context, loggerResult);

        // Act
        var result = (RedirectToActionResult)await controllerResult.DeleteConfirmed(1);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be(nameof(controllerResult.Index));
    }

//     [TestMethod]
//     public async Task LuxuryGet_ShouldRedirectToRent()
//     {
//         // Arrange
//         var context = GetContext();
//         var loggerResult = new Mock<ILogger<ProductsController>>().Object;
//         var rentLoggerResult = new Mock<ILogger<RentController>>().Object;
//         var controllerResult = new ProductsController(context, loggerResult);
//         var rentControllerResult = new RentController(context, rentLoggerResult);

//         // Act
//         var result = (RedirectToActionResult) await controllerResult.LuxuryGet();

//         // Assert
//         result.Should().NotBeNull();
//         result.ActionName.Should().NotBeNull();
//         result.ActionName.Should().Be(nameof(rentControllerResult.Rent));
//     }
}