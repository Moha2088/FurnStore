
using FluentAssertions;
using FurnStore.Data;
using FurnStore.Controllers;
using FurnStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var controllerResult = new ProductsController(context);

        // Act
        var result =  controllerResult.Create();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }
    
    [TestMethod]
    public async Task Create_Should_Return_Index_Page_With_New_Product()
    {
        // Arrange
        var context = GetContext();
            var controllerResult = new ProductsController(context);
        var product = new Product()
        {
            Name = "White Office Chair",
            Material = "Wood",
            Description = "White Chair perfect for office environments or for the dining table ",
            Price = 900,
            ImageUrl = "https://images.unsplash.com/photo-1517705008128-361805f42e86?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTk5fHxtaW5pbWFsJTIwZnVybml0dXJlfGVufDB8fDB8fHww",
            ShippingPrice = 30
        };
        
        // Act
        var result = (RedirectToActionResult) await controllerResult.Create(product);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("Index");
    }
    
    [TestMethod]
    public async Task DeleteConfirmed_Should_Redirect_To_Index()
    {
        // Arrange
        var context = GetContext();
        var controllerResult = new ProductsController(context);

        // Act
        var result = (RedirectToActionResult) await controllerResult.DeleteConfirmed(1);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("Index");
    }
    
}