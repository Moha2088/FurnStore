// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using FluentAssertions;
// using FurnStore.Controllers;
// using FurnStore.Data;
// using FurnStore.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using Xunit;
//
// namespace FurnTestXUnit
// {
//     public class ProductControllerTest
//     {
//         [Fact]
//         public async Task Index_ShouldReturnListOfProducts()
//         {
//             // Arrange
//             var mockProducts = new List<Product>
//             {
//                 new Product
//                 {
//                     Id = 1, Name = "TestProduct", Description = "A test product", Material = "Nothing", Price = 10,
//                     ShippingPrice = 30, ImageUrl = "url"
//                 },
//                 new Product
//                 {
//                     Id = 2, Name = "TestProduct2", Description = "A test product", Material = "Nothing", Price = 10,
//                     ShippingPrice = 30, ImageUrl = "url"
//                 }
//             };
//
//
//             var mockContext = new Mock<FurnStoreContext>();
//             var mockSet = new Mock<DbSet<Product>>();
//             mockContext.Setup(x => x.Product).Returns(mockSet.Object);
//
//             var controller = new ProductsController(mockContext.Object);
//
//             // Act
//             var result = (ViewResult) await controller.Index();
//
//             // Assert
//             result.Should().BeOfType<ViewResult>();
//             result.Model.Should().BeOfType<List<Product>>();
//         }
//     }
// }

