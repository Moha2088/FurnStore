using Microsoft.EntityFrameworkCore;
using FurnStore.Data;
using FurnStore.Models;
using FurnStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

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
}