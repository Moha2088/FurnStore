using System.Security.Claims;
using FurnStore.Data;
using FurnStore.Migrations;
using FurnStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using FurnStore.Interfaces;

namespace FurnStore.Controllers;

public class RentController : Controller, IRent
{
    private readonly FurnStoreContext _context;

    private readonly ILogger<RentController> _logger;

    public RentController(FurnStoreContext context, ILogger<RentController> logger) =>
        (_context, _logger) = (context, logger);

    public async Task<IActionResult> Index()
    {
        var product = await _context.Product
            .Where(p => p.Rentee == null)
            .AsNoTracking()
            .ToListAsync();

        ViewData["ProductCount"] = product.Count();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rent(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _context.Product.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        string userEmail = User.FindFirst(ClaimTypes.Name).Value;

        product.Rentee = userId;
        product.RenteeEmail = userEmail;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelRent(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (product.Rentee == userid)
        {
            product.Rentee = null;
            product.RenteeEmail = null;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(RentedProducts));
    }

    public async Task<IActionResult> RentedProducts()
    {
        var product = await GetProducts();

        var priceSum = product
            .Select(p => p.Price)
            .Sum();

        var shippingPrice = product
            .Select(p => p.ShippingPrice)
            .Distinct()
            .Sum();

        var totalPrice = shippingPrice + priceSum;


        ViewData["PriceSum"] = priceSum;
        ViewData["ProductCount"] = product.Count();
        ViewData["ShippingPrice"] = shippingPrice;
        ViewData["TotalPrice"] = totalPrice;
        return View(product);
    }

    public string? GetUserId()
    {
        string? userId = null;

        if (User.Identity is { IsAuthenticated: true })
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                userId = claim.Value;
            }
        }

        return userId;
    }

    public async Task<List<Product>> GetProducts()
    {
        var userId = GetUserId();
        var product = await _context.Product
            .Where(p => p.Rentee == userId)
            .ToListAsync();

        return product;
    }

    [HttpPost]
    public async Task<IActionResult> ClearList()
    {
        var productsToRemove = await GetProducts();
        productsToRemove.ForEach(product => product.Rentee = null);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(RentedProducts));
    }

    public async Task<IActionResult> GenPdf()
    {
        var product = await GetProducts();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .AlignLeft()
                    .Width(PageSizes.A10.Width)
                    .Image("wwwroot/Images/FurnLogo.png");

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(40);

                        x.Item().Text($"Order Summary/Confirmation #{DateTime.Now}")
                            .SemiBold()
                            .FontSize(22)
                            .FontColor(Colors.Black);

                        x.Item()
                            .Text("Thank you for confirming your order. Below is a list of your ordered products:")
                            .FontSize(14);


                        if (product.Any())
                        {
                            int count = 1;

                            foreach (var item in product)
                            {
                                x.Item()
                                    .Text($"{count} - Name: {item.Name} Price {item.Price}");
                                count++;
                            }

                            var sum = product
                                .Select(x => x.Price)
                                .Sum();

                            sum += product[0].ShippingPrice;
                            x.Item().Text($"Total: {sum}");
                        }

                        else
                        {
                            x.Item()
                                .Text("You have no products in your order list");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span($"Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        document.GeneratePdfAndShow();

        return RedirectToAction(nameof(RentedProducts));
    }

    public async Task LuxuryGet()
    {

    }
}