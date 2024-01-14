using System.Security.Claims;
using FurnStore.Data;
using FurnStore.Migrations;
using FurnStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace FurnStore.Controllers
{
    public class RentController : Controller
    {
        private readonly FurnStoreContext _context;

        public RentController(FurnStoreContext context)
        {
            _context = context;
        }

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
            string? userid = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;

            var product = await _context.Product
                .Where(p => p.Rentee == userid)
                .AsNoTracking()
                .ToListAsync();

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

        [HttpPost]
        public async Task<IActionResult> ClearList()
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var productsToRemove = await _context.Product
                .Where(p => p.Rentee == userid)
                .ToListAsync();

            if (productsToRemove is not null)
            {
                foreach (var product in productsToRemove)
                {
                    product.Rentee = null;
                    await _context.SaveChangesAsync();
                }
            }

            ViewData["ClearedAlert"] = "List has been cleared";
            return RedirectToAction(nameof(RentedProducts));
        }

        public async Task<IActionResult> GenPdf()
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    
                    page.Header()
                        .Text($"Order Summary/Confirmation #{DateTime.Now}")
                        .SemiBold()
                        .FontSize(30)
                        .FontColor(Colors.Black);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(40);

                            x.Item()
                                .Text("Thank you for confirming your order. Below is a list of your ordered products:")
                                .FontSize(14);
                            
                                
                            x.Item()
                                .Text("Product 1 : Chair") // Placeholder
                                .FontSize(12)
                                .Bold();

                            x.Item()
                                .Text("Product 2 : Table") // Placeholder
                                .FontSize(12)
                                .Bold();
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span($"page ");
                            x.CurrentPageNumber();
                        });
                });
                
            }).GeneratePdfAndShow();

            return RedirectToAction(nameof(RentedProducts));
        }
    }
}